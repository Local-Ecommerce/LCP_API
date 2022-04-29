using AutoMapper;
using BLL.Dtos.Resident;
using BLL.Services.Interfaces;
using DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Models;
using BLL.Dtos.Exception;
using DAL.Constants;
using System.Linq;

namespace BLL.Services
{
    public class ResidentService : IResidentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IUtilService _utilService;
        private readonly IValidateDataService _validateDataService;
        private readonly IFirebaseService _firebaseService;
        private const string TYPE = "Profile Image";

        public ResidentService(IUnitOfWork unitOfWork,
            ILogger logger,
            IMapper mapper,
            IUtilService utilService,
            IValidateDataService validateDataService,
            IFirebaseService firebaseService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _utilService = utilService;
            _validateDataService = validateDataService;
            _firebaseService = firebaseService;
        }


        /// <summary>
        /// Create Resident
        /// </summary>
        /// <param name="residentId"></param>
        /// <returns></returns>
        public async Task<Resident> CreateMerchant(string residentId)
        {
            //get account Id
            string accountId = residentId.Substring(0, residentId.IndexOf("_"));

            //Store Resident To Database
            Resident resident = await _unitOfWork.Residents.FindAsync(r => r.AccountId.Equals(accountId));

            Resident residentMerchant = new()
            {
                ResidentId = accountId + "_" + ResidentType.MERCHANT,
                Status = (int)ResidentStatus.UNVERIFIED_RESIDENT,
                CreatedDate = _utilService.CurrentTimeInVietnam(),
                UpdatedDate = _utilService.CurrentTimeInVietnam(),
                AccountId = accountId,
                Type = ResidentType.MERCHANT,
                PhoneNumber = resident.PhoneNumber,
                DateOfBirth = resident.DateOfBirth,
                Gender = resident.Gender,
                ApartmentId = resident.ApartmentId
            };

            return residentMerchant;
        }


        /// <summary>
        /// Update Resident By Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="residentUpdateRequest"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public async Task<ResidentResponse> UpdateResidentById(string id, ResidentUpdateRequest residentUpdateRequest, string role)
        {
            Resident resident;
            Account account;

            //check valid dob
            if (!_validateDataService.IsLaterThanPresent(residentUpdateRequest.DateOfBirth))
            {
                _logger.Error($"[Invalid Date Of Birth : '{residentUpdateRequest.DateOfBirth}']");

                throw new BusinessException(ResidentStatus.INVALID_DATE_OF_BIRTH_RESIDENT.ToString(), (int)ResidentStatus.INVALID_DATE_OF_BIRTH_RESIDENT);
            }

            //Check id
            try
            {
                resident = await _unitOfWork.Residents.FindAsync(resident => resident.ResidentId.Equals(id));
                account = await _unitOfWork.Accounts.FindAsync(a => a.AccountId.Equals(resident.AccountId));
            }
            catch (Exception e)
            {
                _logger.Error("[ResidentService.UpdateResidentById()]: " + e.Message);

                throw new EntityNotFoundException(typeof(Resident), id);
            }

            //update Resident
            try
            {
                resident = _mapper.Map(residentUpdateRequest, resident);
                resident.UpdatedDate = _utilService.CurrentTimeInVietnam();
                if (role.Equals(ResidentType.CUSTOMER))
                    resident.Status = (int)ResidentStatus.UNVERIFIED_RESIDENT;

                _unitOfWork.Residents.Update(resident);

                if (residentUpdateRequest.ProfileImage != null)
                {
                    account.ProfileImage = _firebaseService
                                    .UploadFileToFirebase(residentUpdateRequest.ProfileImage, TYPE, account.AccountId, "Image")
                                    .Result;
                    _unitOfWork.Accounts.Update(account);
                }

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[ResidentService.UpdateMerchantById()]: " + e.Message);

                throw;
            }

            return _mapper.Map<ResidentResponse>(resident);
        }


        /// <summary>
        /// Delete Resident
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteResident(string id)
        {
            //biz rule

            //Check id
            Resident resident;
            try
            {
                resident = await _unitOfWork.Residents.FindAsync(resident => resident.ResidentId.Equals(id));
            }
            catch (Exception e)
            {
                _logger.Error("[ResidentService.DeleteResident()]: " + e.Message);

                throw new EntityNotFoundException(typeof(Resident), id);
            }

            //Delete Resident
            try
            {
                resident.Status = (int)ResidentStatus.DELETED_RESIDENT;

                _unitOfWork.Residents.Update(resident);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[ResidentService.DeleteResident()]: " + e.Message);

                throw;
            }
        }


        /// <summary>
        /// Get Resident
        /// </summary>
        /// <param name="id"></param>
        /// <param name="apartmentId"></param>
        /// <param name="accountId"></param>
        /// <param name="status"></param>
        /// <param name="type"></param>
        /// <param name="limit"></param>
        /// <param name="page"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public async Task<object> GetResident(
            string id, string apartmentId,
            string accountId, int?[] status, string type, int? limit,
            int? page, string sort)
        {
            PagingModel<Resident> residents;
            string propertyName = default;
            bool isAsc = false;

            if (!string.IsNullOrEmpty(sort))
            {
                isAsc = sort[0].ToString().Equals("+");
                propertyName = _utilService.UpperCaseFirstLetter(sort[1..]);
            }

            try
            {
                residents = await _unitOfWork.Residents
                    .GetResident(id, apartmentId, accountId, status, type, limit, page, isAsc, propertyName);
            }
            catch (Exception e)
            {
                _logger.Error("[ResidentService.GetResident()]" + e.Message);
                throw;
            }

            return new PagingModel<ExtendResidentResponse>
            {
                List = _mapper.Map<List<ExtendResidentResponse>>(residents.List),
                Page = residents.Page,
                LastPage = residents.LastPage,
                Total = residents.Total,
            };
        }


        /// <summary>
        /// Update Resident Status
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <param name="managerId"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public async Task<ExtendResidentResponse> UpdateResidentStatus(string id, int status, string managerId, string role)
        {
            Resident resident;
            try
            {
                List<Resident> residents = await _unitOfWork.Residents
                    .FindListAsync(r => r.ResidentId.Equals(id) || r.ResidentId.Equals(managerId));

                //get resident need to update
                resident = residents.Where(r => r.ResidentId.Equals(id)).First();

                if (role.Equals(ResidentType.MARKET_MANAGER))
                    //check apartment of Market Manager
                    if (!residents.Where(r => r.ResidentId.Equals(managerId))
                                .First().ApartmentId
                                .Equals(resident.ApartmentId))
                        throw new BusinessException($"Không thể cập nhật trạng thái cho cư dân này.");

                //check status
                if (status != (int)ResidentStatus.VERIFIED_RESIDENT &&
                status != (int)ResidentStatus.REJECTED_RESIDENT &&
                status != (int)ResidentStatus.INACTIVE_RESIDENT)
                    throw new BusinessException($"Không thể cập nhật trạng thái cho cư dân này.");

                resident.Status = status;
                resident.ApproveBy = status != (int)ResidentStatus.REJECTED_RESIDENT ? managerId : null;

                _unitOfWork.Residents.Update(resident);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[ResidentService.VerifyResident()]: " + e.Message);
                throw;
            }

            return _mapper.Map<ExtendResidentResponse>(resident);
        }


        /// <summary>
        /// Create Guest
        /// </summary>
        /// <param name="guest"></param>
        /// <param name="apartmentId"></param>
        /// <param name="marketManagerId"></param>
        /// <returns></returns>
        public async Task<Resident> CreateGuest(ResidentGuest guest, string apartmentId, string marketManagerId)
        {
            Resident resident;

            try
            {
                //get resident by phone number
                if (_validateDataService.IsVietnamesePhoneNumber(guest.PhoneNumber))
                {
                    resident = await _unitOfWork.Residents.FindAsync(r => r.PhoneNumber.Equals(guest.PhoneNumber));
                    if (resident != null)
                        throw new BusinessException("Đã có cư dân sử dụng số điện thoại này");
                }

                resident = new Resident
                {
                    ResidentId = _utilService.CreateId("") + "_GUEST",
                    PhoneNumber = guest.PhoneNumber,
                    ResidentName = guest.ResidentName,
                    DeliveryAddress = guest.DeliveryAddress,
                    Type = ResidentType.CUSTOMER,
                    CreatedDate = _utilService.CurrentTimeInVietnam(),
                    UpdatedDate = _utilService.CurrentTimeInVietnam(),
                    Status = (int)ResidentStatus.VERIFIED_RESIDENT,
                    ApartmentId = apartmentId,
                    ApproveBy = marketManagerId,
                    AccountId = null
                };

                _unitOfWork.Residents.Add(resident);
            }
            catch (Exception e)
            {
                _logger.Error("[ResidentService.CreateGuest()" + e.Message);
                throw;
            }

            return resident;
        }
    }
}
