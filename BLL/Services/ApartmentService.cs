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
        public async Task<BaseResponse<ApartmentResponse>> CreateApartment(ApartmentRequest apartmentRequest)
        {

            //biz rule

            //Store Apartment To Dabatabase
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
                apartment = await _unitOfWork.Apartments.FindAsync(ap => ap.ApartmentId.Equals(id));
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

                _unitOfWork.Apartments.Update(apartment);

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


            ApartmentResponse apartmentResponse;

            //Get Apartment From Database

            try
            {
                Apartment apartment = await _unitOfWork.Apartments.FindAsync(ap => ap.ApartmentId.Equals(id));

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
                apartment = await _unitOfWork.Apartments.FindAsync(ap => ap.ApartmentId.Equals(id));
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

                _unitOfWork.Apartments.Update(apartment);

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

            return new BaseResponse<ApartmentResponse>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = apartmentResponse
            };
        }


        /// <summary>
        /// Get Apartment By Address
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        /// <exception cref="HttpStatusException"></exception>
        public async Task<BaseResponse<ApartmentResponse>> GetApartmentByAddress(string address)
        {
            //biz rule


            ApartmentResponse apartmentResponse;

            //Get Apartment From Database

            try
            {
                Apartment apartment = await _unitOfWork.Apartments.FindAsync(ap => ap.Address.Equals(address));

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

            return new BaseResponse<ApartmentResponse>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = apartmentResponse
            };
        }


        /// <summary>
        /// Get Apartments By Status
        /// </summary>
        /// <returns></returns>
        /// <exception cref="HttpStatusException"></exception>
        public async Task<BaseResponse<List<ApartmentResponse>>> GetApartmentsByStatus(int status)
        {
            //biz rule


            List<ApartmentResponse> apartmentResponses;

            //Get Apartment From Database

            try
            {
                List<Apartment> apartments = await _unitOfWork.Apartments.FindListAsync(ap => ap.Status == status);

                apartmentResponses = _mapper.Map<List<ApartmentResponse>>(apartments);
            }
            catch (Exception e)
            {
                _logger.Error("[ApartmentService.GetApartmentsByStatus()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<ApartmentResponse>
                    {
                        ResultCode = (int)ApartmentStatus.APARTMENT_NOT_FOUND,
                        ResultMessage = ApartmentStatus.APARTMENT_NOT_FOUND.ToString(),
                        Data = default
                    });
            }

            return new BaseResponse<List<ApartmentResponse>>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = apartmentResponses
            };
        }


        /// <summary>
        /// Get Apartment For Auto Complete
        /// </summary>
        /// <returns></returns>
        /// <exception cref="HttpStatusException"></exception>
        public async Task<BaseResponse<List<ApartmentResponse>>> GetApartmentForAutoComplete()
        {
            //biz rule


            List<ApartmentResponse> apartmentResponses;

            //Get Apartment From Database

            try
            {
                List<Apartment> apartments = await _unitOfWork.Apartments.GetAllActiveApartment();

                apartmentResponses = _mapper.Map<List<ApartmentResponse>>(apartments);
            }
            catch (Exception e)
            {
                _logger.Error("[ApartmentService.GetApartmentForAutoComplete()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<ApartmentResponse>
                    {
                        ResultCode = (int)ApartmentStatus.APARTMENT_NOT_FOUND,
                        ResultMessage = ApartmentStatus.APARTMENT_NOT_FOUND.ToString(),
                        Data = default
                    });
            }

            return new BaseResponse<List<ApartmentResponse>>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = apartmentResponses
            };
        }


        /// <summary>
        /// Get All Apartments
        /// </summary>
        /// <returns></returns>
        /// <exception cref="HttpStatusException"></exception>
        public async Task<BaseResponse<List<ApartmentResponse>>> GetAllApartments()
        {
            //biz rule


            List<ApartmentResponse> apartmentResponses;

            //Get Apartment From Database

            try
            {
                List<Apartment> apartments = await _unitOfWork.Apartments.GetAllApartment();

                apartmentResponses = _mapper.Map<List<ApartmentResponse>>(apartments);
            }
            catch (Exception e)
            {
                _logger.Error("[ApartmentService.GetAllApartments()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<ApartmentResponse>
                    {
                        ResultCode = (int)ApartmentStatus.APARTMENT_NOT_FOUND,
                        ResultMessage = ApartmentStatus.APARTMENT_NOT_FOUND.ToString(),
                        Data = default
                    });
            }

            return new BaseResponse<List<ApartmentResponse>>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = apartmentResponses
            };
        }
    }
}
