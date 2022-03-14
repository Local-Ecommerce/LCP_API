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


        public ResidentService(IUnitOfWork unitOfWork,
            ILogger logger,
            IMapper mapper,
            IUtilService utilService,
            IValidateDataService validateDataService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _utilService = utilService;
            _validateDataService = validateDataService;
        }


        /// <summary>
        /// Create Resident
        /// </summary>
        /// <param name="residentRequest"></param>
        /// <param name="residentId"></param>
        /// <returns></returns>
        public async Task<ResidentResponse> CreateResident(ResidentRequest residentRequest, string residentId)
        {
            //check valid dob
            if (!_validateDataService.IsLaterThanPresent(residentRequest.DateOfBirth))
            {
                _logger.Error($"[Invalid Date Of Birth : '{residentRequest.DateOfBirth}']");

                throw new BusinessException(ResidentStatus.INVALID_DATE_OF_BIRTH_RESIDENT.ToString(), (int)ResidentStatus.INVALID_DATE_OF_BIRTH_RESIDENT);
            }

            //get account Id
            string accountId = residentId.Substring(0, residentId.IndexOf("_"));

            //Store Resident To Database
            Resident resident = _mapper.Map<Resident>(residentRequest);
            try
            {
                resident.ResidentId = accountId + "_" + ResidentType.MERCHANT;
                resident.Status = (int)ResidentStatus.UNVERIFIED_RESIDENT;
                resident.CreatedDate = DateTime.Now;
                resident.AccountId = accountId;
                resident.Type = ResidentType.MERCHANT;

                _unitOfWork.Residents.Add(resident);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[ResidentService.CreateResident()]: " + e.Message);

                throw;
            }

            return _mapper.Map<ResidentResponse>(resident);
        }


        /// <summary>
        /// Update Resident By Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="residentUpdateRequest"></param>
        /// <returns></returns>
        public async Task<ResidentResponse> UpdateResidentById(string id, ResidentUpdateRequest residentUpdateRequest)
        {
            Resident resident;

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
                resident.UpdatedDate = DateTime.Now;

                _unitOfWork.Residents.Update(resident);

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
        /// <param name="limit"></param>
        /// <param name="page"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public async Task<object> GetResident(
            string id, string apartmentId,
            string accountId, int? limit,
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
                residents = await _unitOfWork.Residents.GetResident(id, apartmentId, accountId, limit, page, isAsc, propertyName);
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
        /// <param name="marketManagerId"></param>
        /// <returns></returns>
        public async Task<ExtendResidentResponse> UpdateResidentStatus(string id, int status, string marketManagerId)
        {
            Resident resident;
            try
            {
                List<Resident> residents = await _unitOfWork.Residents
                    .FindListAsync(r => r.ResidentId.Equals(id) || r.ResidentId.Equals(marketManagerId));

                //get resident need to update
                resident = residents.Where(r => r.ResidentId.Equals(id)).First();

                //check apartment of Market Manager
                if (!residents.Where(r => r.ResidentId.Equals(marketManagerId))
                            .First().ApartmentId
                            .Equals(resident.ApartmentId))
                    throw new BusinessException($"MarketManager with id: {marketManagerId} cannot update resident {id} 's status");

                //check status
                if (status != (int)ResidentStatus.VERIFIED_RESIDENT &&
                status != (int)ResidentStatus.REJECTED_RESIDENT &&
                status != (int)ResidentStatus.INACTIVE_RESIDENT)
                    throw new BusinessException($"Cannot update status {status} for resident {id}");

                resident.Status = status;
                resident.ApproveBy = status != (int)ResidentStatus.REJECTED_RESIDENT ? marketManagerId : default;

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
    }
}
