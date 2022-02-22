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

namespace BLL.Services
{
    public class ResidentService : IResidentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IUtilService _utilService;
        private readonly IValidateDataService _validateDataService;
        private const string PREFIX = "RS_";


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
        /// <returns></returns>
        public async Task<ResidentResponse> CreateResident(ResidentRequest residentRequest)
        {
            //biz rule

            //check valid Resident's Name
            if (!_validateDataService.IsValidName(residentRequest.ResidentName))
            {
                _logger.Error($"[Invalid Name : '{residentRequest.ResidentName}']");

                throw new BusinessException(ResidentStatus.INVALID_NAME_RESIDENT.ToString(), (int)ResidentStatus.INVALID_NAME_RESIDENT);

            }

            //check valid Resident's Type
            if (!residentRequest.Type.Equals(ResidentType.MERCHANT) 
                && !residentRequest.Type.Equals(ResidentType.MARKET_MANAGER)
                && !residentRequest.Type.Equals(ResidentType.CUSTOMER))
            {
                _logger.Error($"[Invalid Resident Type : '{residentRequest.Type}']");

                throw new BusinessException(ResidentStatus.INVALID_TYPE_RESIDENT.ToString(), (int)ResidentStatus.INVALID_TYPE_RESIDENT);

            }

            //check valid Resident's PhoneNumber
            if (!_validateDataService.IsValidPhoneNumber(residentRequest.PhoneNumber))
            {
                _logger.Error($"[Invalid PhoneNumber : '{residentRequest.PhoneNumber}']");

                throw new BusinessException(ResidentStatus.INVALID_PHONE_NUMBER_RESIDENT.ToString(), (int)ResidentStatus.INVALID_PHONE_NUMBER_RESIDENT);

            }

            //check valid dob
            if (!_validateDataService.IsLaterThanPresent(residentRequest.DateOfBirth))
            {
                _logger.Error($"[Invalid Date Of Birth : '{residentRequest.DateOfBirth}']");

                throw new BusinessException(ResidentStatus.INVALID_DATE_OF_BIRTH_RESIDENT.ToString(), (int)ResidentStatus.INVALID_DATE_OF_BIRTH_RESIDENT);

            }

            //Store Resident To Database
            Resident resident = _mapper.Map<Resident>(residentRequest);
            try
            {
                resident.ResidentId = _utilService.CreateId(PREFIX);
                resident.Status = (int)ResidentStatus.ACTIVE_RESIDENT;
                resident.CreatedDate = DateTime.Now;

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

            //check valid Resident's Name
            if (!_validateDataService.IsValidName(residentUpdateRequest.ResidentName))
            {
                _logger.Error($"[Invalid Name : '{residentUpdateRequest.ResidentName}']");

                throw new BusinessException(ResidentStatus.INVALID_NAME_RESIDENT.ToString(), (int)ResidentStatus.INVALID_NAME_RESIDENT);

            }

            //check valid Resident's PhoneNumber
            if (!_validateDataService.IsValidPhoneNumber(residentUpdateRequest.PhoneNumber))
            {
                _logger.Error($"[Invalid PhoneNumber : '{residentUpdateRequest.PhoneNumber}']");

                throw new BusinessException(ResidentStatus.INVALID_PHONE_NUMBER_RESIDENT.ToString(), (int)ResidentStatus.INVALID_PHONE_NUMBER_RESIDENT);

            }

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
                resident.Status = (int)ResidentStatus.ACTIVE_RESIDENT;

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
        public async Task<ResidentResponse> DeleteResident(string id)
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

            return _mapper.Map<ResidentResponse>(resident);
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

                if (_utilService.IsNullOrEmpty(residents.List))
                    throw new EntityNotFoundException(typeof(Resident), "in the url");
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
    }
}
