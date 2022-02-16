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
        /// Get Resident By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ResidentResponse> GetResidentById(string id)
        {
            //biz rule

            ResidentResponse residentResponse;

            //Get Resident From Database
            try
            {
                Resident resident = await _unitOfWork.Residents.FindAsync(resident => resident.ResidentId.Equals(id));
                residentResponse = _mapper.Map<ResidentResponse>(resident);
            }
            catch (Exception e)
            {
                _logger.Error("[ResidentService.GetResidentById()]: " + e.Message);

                throw new EntityNotFoundException(typeof(Resident), id);
            }

            return residentResponse;
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
        /// Get Resident By Apartment Id
        /// </summary>
        /// <param name="apartmentId"></param>
        /// <returns></returns>
        public async Task<List<ResidentResponse>> GetResidentByApartmentId(string apartmentId)
        {
            List<ResidentResponse> residentResponses;

            //Get Resident From Database

            try
            {
                List<Resident> residentList = await _unitOfWork.Residents
                                                            .FindListAsync(resident => resident.ApartmentId.Equals(apartmentId));

                residentResponses = _mapper.Map<List<ResidentResponse>>(residentList);
            }
            catch (Exception e)
            {
                _logger.Error("[ResidentService.GetResidentByAppartmentId()]: " + e.Message);

                throw new EntityNotFoundException(typeof(Resident), apartmentId);
            }

            return residentResponses;
        }


        /// <summary>
        /// Get All Residents
        /// </summary>
        /// <returns></returns>
        public async Task<List<ResidentResponse>> GetAllResidents()
        {
            List<ResidentResponse> residentResponses;

            //Get Resident From Database

            try
            {
                List<Resident> residentList = await _unitOfWork.Residents
                                                            .FindListAsync(resident => resident.ResidentId != null);

                residentResponses = _mapper.Map<List<ResidentResponse>>(residentList);
            }
            catch (Exception e)
            {
                _logger.Error("[ResidentService.GetAllResidents()]: " + e.Message);

                throw new EntityNotFoundException(typeof(Resident), "all");
            }

            return residentResponses;
        }


        /// <summary>
        /// Get Resident By Account Id
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public async Task<List<ResidentResponse>> GetResidentByAccountId(string accountId)
        {
            List<ResidentResponse> residentResponses;

            //Get Resident From Database

            try
            {
                List<Resident> residentList = await _unitOfWork.Residents
                                                            .FindListAsync(resident => resident.AccountId.Equals(accountId));

                residentResponses = _mapper.Map<List<ResidentResponse>>(residentList);
            }
            catch (Exception e)
            {
                _logger.Error("[ResidentService.GetResidentByAccountId()]: " + e.Message);

                throw new EntityNotFoundException(typeof(Resident), accountId);
            }

            return residentResponses;
        }
    }
}
