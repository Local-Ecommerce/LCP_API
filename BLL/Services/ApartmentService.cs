﻿using AutoMapper;
using BLL.Dtos;
using BLL.Constants;
using BLL.Dtos.Exception;
using BLL.Dtos.Apartment;
using BLL.Services.Interfaces;
using DAL.Models;
using DAL.UnitOfWork;
using System;
using System.Net;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class ApartmentService : IApartmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IRedisService _redisService;
        private readonly IUtilService _utilService;
        private const string CACHE_KEY = "Apartment";

        public ApartmentService(IUnitOfWork unitOfWork,
            ILogger logger,
            IMapper mapper,
            IRedisService redisService,
            IUtilService utilService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _redisService = redisService;
            _utilService = utilService;
        }


        /// <summary>
        /// Create Apartment
        /// </summary>
        /// <param name="apartmentRequest"></param>
        /// <returns></returns>
        public async Task<BaseResponse<ApartmentResponse>> CreateApartment(ApartmentRequest apartmentRequest)
        {

            //biz rule

            //Store Apartment To Dabatabase
            Apartment apartment = _mapper.Map<Apartment>(apartmentRequest);

            try
            {
                apartment.ApartmentId = _utilService.Create16Alphanumeric();
                apartment.Status = (int)ApartmentStatus.ACTIVE_APARTMENT;

                _unitOfWork.Repository<Apartment>().Add(apartment);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[ApartmentService.CreateApartment()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<ApartmentResponse>
                    {
                        ResultCode = (int)CommonResponse.ERROR,
                        ResultMessage = CommonResponse.ERROR.ToString(),
                        Data = default
                    });
            }

            //Create Response
            ApartmentResponse apartmentResponse = _mapper.Map<ApartmentResponse>(apartment);

            //Store Apartment To Redis
            _redisService.StoreToList(CACHE_KEY, apartmentResponse,
                    new Predicate<ApartmentResponse>(a => a.ApartmentId == apartmentResponse.ApartmentId));

            return new BaseResponse<ApartmentResponse>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = apartmentResponse
            };

        }


        /// <summary>
        /// Delete Apartment
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<BaseResponse<ApartmentResponse>> DeleteApartment(string id)
        {
            //biz rule

            //Check id
            Apartment apartment;
            try
            {
                apartment = await _unitOfWork.Repository<Apartment>()
                                       .FindAsync(local => local.ApartmentId.Equals(id));
            }
            catch (Exception e)
            {
                _logger.Error("[ApartmentService.DeleteApartment()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<Apartment>
                    {
                        ResultCode = (int)ApartmentStatus.APARTMENT_NOT_FOUND,
                        ResultMessage = ApartmentStatus.APARTMENT_NOT_FOUND.ToString(),
                        Data = default
                    });
            }

            //Delete Apartment
            try
            {
                apartment.Status = (int)ApartmentStatus.DELETED_APARTMENT;

                _unitOfWork.Repository<Apartment>().Update(apartment);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[ApartmentService.DeleteApartment()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<Apartment>
                    {
                        ResultCode = (int)CommonResponse.ERROR,
                        ResultMessage = CommonResponse.ERROR.ToString(),
                        Data = default
                    });
            }

            //Create Response
            ApartmentResponse apartmentResponse = _mapper.Map<ApartmentResponse>(apartment);

            //Store Apartment To Redis
            _redisService.StoreToList(CACHE_KEY, apartmentResponse,
                    new Predicate<ApartmentResponse>(a => a.ApartmentId == apartmentResponse.ApartmentId));

            return new BaseResponse<ApartmentResponse>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = apartmentResponse
            };

        }


        /// <summary>
        /// Get Apartment By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<BaseResponse<ApartmentResponse>> GetApartmentById(string id)
        {
            //biz rule


            ApartmentResponse apartmentResponse = null;
            //Get Apartment From Redis
            apartmentResponse = _redisService.GetList<ApartmentResponse>(CACHE_KEY)
                                            .Find(local => local.ApartmentId.Equals(id));

            //Get Apartment From Database
            if(apartmentResponse is null)
            {
                try
                {
                    Apartment apartment = await _unitOfWork.Repository<Apartment>().
                                                            FindAsync(local => local.ApartmentId.Equals(id));

                    apartmentResponse = _mapper.Map<ApartmentResponse>(apartment);
                }
                catch (Exception e)
                {
                    _logger.Error("[ApartmentService.GetApartmentById()]: " + e.Message);

                    throw new HttpStatusException(HttpStatusCode.OK,
                        new BaseResponse<ApartmentResponse>
                        {
                            ResultCode = (int)ApartmentStatus.APARTMENT_NOT_FOUND,
                            ResultMessage = ApartmentStatus.APARTMENT_NOT_FOUND.ToString(),
                            Data = default
                        });
                }
            }

            return new BaseResponse<ApartmentResponse>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = apartmentResponse
            };
        }


        /// <summary>
        /// Update Apartment
        /// </summary>
        /// <param name="id"></param>
        /// <param name="apartmentRequest"></param>
        /// <returns></returns>
        public async Task<BaseResponse<ApartmentResponse>> UpdateApartmentById(string id, ApartmentRequest apartmentRequest)
        {
            //biz ruie

            //Check id
            Apartment apartment;
            try
            {
                apartment = await _unitOfWork.Repository<Apartment>()
                                       .FindAsync(local => local.ApartmentId.Equals(id));
            }
            catch (Exception e)
            {
                _logger.Error("[ApartmentService.UpdateApartment()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK, new BaseResponse<ApartmentResponse>
                {
                    ResultCode = (int)ApartmentStatus.APARTMENT_NOT_FOUND,
                    ResultMessage = ApartmentStatus.APARTMENT_NOT_FOUND.ToString(),
                    Data = default
                });
            }

            //Update Apartment To DB
            try
            {
                apartment = _mapper.Map(apartmentRequest, apartment);

                _unitOfWork.Repository<Apartment>().Update(apartment);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[ApartmentService.UpdateApartment()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK, new BaseResponse<ApartmentResponse>
                {
                    ResultCode = (int)CommonResponse.ERROR,
                    ResultMessage = CommonResponse.ERROR.ToString(),
                    Data = default
                });
            }

            //Create Response
            ApartmentResponse apartmentResponse = _mapper.Map<ApartmentResponse>(apartment);

            //Store Reponse To Redis
            _redisService.StoreToList(CACHE_KEY, apartmentResponse,
                    new Predicate<ApartmentResponse>(a => a.ApartmentId == apartmentResponse.ApartmentId));

            return new BaseResponse<ApartmentResponse>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = apartmentResponse
            };
        }

        public async Task<BaseResponse<ApartmentResponse>> GetApartmentByAddress(string address)
        {
            //biz rule


            ApartmentResponse apartmentResponse = null;
            //Get Apartment From Redis
            apartmentResponse = _redisService.GetList<ApartmentResponse>(CACHE_KEY)
                                            .Find(local => local.Address.Equals(address));

            //Get Apartment From Database
            if (apartmentResponse is null)
            {
                try
                {
                    Apartment apartment = await _unitOfWork.Repository<Apartment>().
                                                            FindAsync(local => local.Address.Equals(address));

                    apartmentResponse = _mapper.Map<ApartmentResponse>(apartment);
                }
                catch (Exception e)
                {
                    _logger.Error("[ApartmentService.GetApartmentByAddress()]: " + e.Message);

                    throw new HttpStatusException(HttpStatusCode.OK,
                        new BaseResponse<ApartmentResponse>
                        {
                            ResultCode = (int)ApartmentStatus.APARTMENT_NOT_FOUND,
                            ResultMessage = ApartmentStatus.APARTMENT_NOT_FOUND.ToString(),
                            Data = default
                        });
                }
            }

            return new BaseResponse<ApartmentResponse>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = apartmentResponse
            };
        }
    }
}