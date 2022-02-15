using AutoMapper;
using BLL.Dtos;
using DAL.Constants;
using BLL.Dtos.Exception;
using BLL.Dtos.Apartment;
using BLL.Services.Interfaces;
using DAL.Models;
using DAL.UnitOfWork;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BLL.Services
{
    public class ApartmentService : IApartmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IUtilService _utilService;
        private const string PREFIX = "APM_";

        public ApartmentService(IUnitOfWork unitOfWork,
            ILogger logger,
            IMapper mapper,
            IUtilService utilService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _utilService = utilService;
        }


        /// <summary>
        /// Create Apartment
        /// </summary>
        /// <param name="apartmentRequest"></param>
        /// <returns></returns>
        public async Task<ApartmentResponse> CreateApartment(ApartmentRequest apartmentRequest)
        {
            Apartment apartment = _mapper.Map<Apartment>(apartmentRequest);
            try
            {
                apartment.ApartmentId = _utilService.CreateId(PREFIX);
                apartment.Status = (int)ApartmentStatus.ACTIVE_APARTMENT;

                _unitOfWork.Apartments.Add(apartment);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[ApartmentService.CreateApartment()]: " + e.Message);
                throw;
            }

            return _mapper.Map<ApartmentResponse>(apartment);
        }


        /// <summary>
        /// Delete Apartment
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ApartmentResponse> DeleteApartment(string id)
        {
            //Check id
            Apartment apartment;
            try
            {
                apartment = await _unitOfWork.Apartments.FindAsync(ap => ap.ApartmentId.Equals(id));
            }
            catch (Exception e)
            {
                _logger.Error("[ApartmentService.DeleteApartment()]: " + e.Message);

                throw new EntityNotFoundException(typeof(Apartment), id);
            }

            //Delete Apartment
            try
            {
                apartment.Status = (int)ApartmentStatus.DELETED_APARTMENT;

                _unitOfWork.Apartments.Update(apartment);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[ApartmentService.DeleteApartment()]: " + e.Message);
                throw;
            }

            return _mapper.Map<ApartmentResponse>(apartment);
        }


        /// <summary>
        /// Get Apartment By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ApartmentResponse> GetApartmentById(string id)
        {
            ApartmentResponse apartmentResponse;
            try
            {
                Apartment apartment = await _unitOfWork.Apartments.FindAsync(ap => ap.ApartmentId.Equals(id));

                apartmentResponse = _mapper.Map<ApartmentResponse>(apartment);
            }
            catch (Exception e)
            {
                _logger.Error("[ApartmentService.GetApartmentById()]: " + e.Message);

                throw new EntityNotFoundException(typeof(Apartment), id);
            }

            return apartmentResponse;
        }


        /// <summary>
        /// Update Apartment
        /// </summary>
        /// <param name="id"></param>
        /// <param name="apartmentRequest"></param>
        /// <returns></returns>
        public async Task<ApartmentResponse> UpdateApartmentById(string id, ApartmentRequest apartmentRequest)
        {
            //biz ruie

            //Check id
            Apartment apartment;
            try
            {
                apartment = await _unitOfWork.Apartments.FindAsync(ap => ap.ApartmentId.Equals(id));
            }
            catch (Exception e)
            {
                _logger.Error("[ApartmentService.UpdateApartment()]: " + e.Message);

                throw new EntityNotFoundException(typeof(Apartment), id);
            }

            //Update Apartment To DB
            try
            {
                apartment = _mapper.Map(apartmentRequest, apartment);

                _unitOfWork.Apartments.Update(apartment);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[ApartmentService.UpdateApartment()]: " + e.Message);
                throw;
            }

            return _mapper.Map<ApartmentResponse>(apartment);
        }


        /// <summary>
        /// Get Apartment By Address
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public async Task<ApartmentResponse> GetApartmentByAddress(string address)
        {
            ApartmentResponse apartmentResponse;
            try
            {
                Apartment apartment = await _unitOfWork.Apartments.FindAsync(ap => ap.Address.Equals(address));

                apartmentResponse = _mapper.Map<ApartmentResponse>(apartment);
            }
            catch (Exception e)
            {
                _logger.Error("[ApartmentService.GetApartmentByAddress()]: " + e.Message);

                throw new EntityNotFoundException(typeof(Apartment), address);
            }

            return apartmentResponse;
        }


        /// <summary>
        /// Get Apartments By Status
        /// </summary>
        /// <returns></returns>
        /// <exception cref="HttpStatusException"></exception>
        public async Task<List<ApartmentResponse>> GetApartmentsByStatus(int status)
        {
            List<ApartmentResponse> apartmentResponses;
            try
            {
                List<Apartment> apartments = await _unitOfWork.Apartments.FindListAsync(ap => ap.Status == status);

                apartmentResponses = _mapper.Map<List<ApartmentResponse>>(apartments);
            }
            catch (Exception e)
            {
                _logger.Error("[ApartmentService.GetApartmentsByStatus()]: " + e.Message);

                throw new EntityNotFoundException(typeof(Apartment), status);
            }

            return apartmentResponses;
        }


        /// <summary>
        /// Get Apartment For Auto Complete
        /// </summary>
        /// <returns></returns>
        public async Task<List<ApartmentResponse>> GetApartmentForAutoComplete()
        {
            List<ApartmentResponse> apartmentResponses;
            try
            {
                List<Apartment> apartments = await _unitOfWork.Apartments.GetAllActiveApartment();

                apartmentResponses = _mapper.Map<List<ApartmentResponse>>(apartments);
            }
            catch (Exception e)
            {
                _logger.Error("[ApartmentService.GetApartmentForAutoComplete()]: " + e.Message);

                throw new EntityNotFoundException(typeof(Apartment), "autocomplete");
            }

            return apartmentResponses;
        }


        /// <summary>
        /// Get All Apartments
        /// </summary>
        /// <returns></returns>
        public async Task<List<ApartmentResponse>> GetAllApartments()
        {
            List<ApartmentResponse> apartmentResponses;
            try
            {
                List<Apartment> apartments = await _unitOfWork.Apartments.GetAllApartment();

                apartmentResponses = _mapper.Map<List<ApartmentResponse>>(apartments);
            }
            catch (Exception e)
            {
                _logger.Error("[ApartmentService.GetAllApartments()]: " + e.Message);

                throw new EntityNotFoundException(typeof(Apartment), "all");
            }

            return apartmentResponses;
        }


        /// <summary>
        /// Get Market Manager By Apartment Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ExtendApartmentResponse> GetMarketManagerByApartmentId(string id)
        {
            ExtendApartmentResponse apartmentResponse;
            try
            {
                Apartment apartment = await _unitOfWork.Apartments.GetMarketManagerByApartmentId(id);

                apartmentResponse = _mapper.Map<ExtendApartmentResponse>(apartment);
            }
            catch (Exception e)
            {
                _logger.Error("[ApartmentService.GetMarketManagerByApartmentId()]: " + e.Message);

                throw new EntityNotFoundException(typeof(Resident), id);
            }

            return apartmentResponse;
        }
    }
}
