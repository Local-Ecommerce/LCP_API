using AutoMapper;
using BLL.Constants;
using BLL.Dtos;
using BLL.Dtos.DeliveryAddress;
using BLL.Dtos.Exception;
using BLL.Services.Interfaces;
using DAL.Models;
using DAL.UnitOfWork;
using System;
using System.Net;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class DeliveryAddressService : IDeliveryAddressService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IUtilService _utilService;
        private const string PREFIX = "DA_";

        public DeliveryAddressService(IUnitOfWork unitOfWork,
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
        /// Create DeliveryAddress
        /// </summary>
        /// <param name="deliveryAddressRequest"></param>
        /// <returns></returns>
        public async Task<BaseResponse<DeliveryAddressResponse>> CreateDeliveryAddress(DeliveryAddressRequest deliveryAddressRequest)
        {

            //biz rule

            //Store DeliveryAddress To Dabatabase
            DeliveryAddress deliveryAddress = _mapper.Map<DeliveryAddress>(deliveryAddressRequest);

            try
            {
                deliveryAddress.DeliveryAddressId = _utilService.CreateId(PREFIX);
                deliveryAddress.IsPrimaryAddress = false;
                _unitOfWork.Repository<DeliveryAddress>().Add(deliveryAddress);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[DeliveryAddressService.CreateDeliveryAddress()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<DeliveryAddressResponse>
                    {
                        ResultCode = (int)CommonResponse.ERROR,
                        ResultMessage = CommonResponse.ERROR.ToString(),
                        Data = default
                    });
            }

            //Create Response
            DeliveryAddressResponse deliveryAddressResponse = _mapper.Map<DeliveryAddressResponse>(deliveryAddress);

            return new BaseResponse<DeliveryAddressResponse>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = deliveryAddressResponse
            };

        }


        /// <summary>
        /// Delete DeliveryAddress
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<BaseResponse<DeliveryAddressResponse>> DeleteDeliveryAddress(string id)
        {
            //biz rule

            //Check id
            DeliveryAddress deliveryAddress;
            try
            {
                deliveryAddress = await _unitOfWork.Repository<DeliveryAddress>()
                                       .FindAsync(local => local.DeliveryAddressId.Equals(id));
            }
            catch (Exception e)
            {
                _logger.Error("[DeliveryAddressService.DeleteDeliveryAddress()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<DeliveryAddress>
                    {
                        ResultCode = (int)DeliveryAddressStatus.DELIVERYADDRESS_NOT_FOUND,
                        ResultMessage = DeliveryAddressStatus.DELIVERYADDRESS_NOT_FOUND.ToString(),
                        Data = default
                    });
            }

            //Delete DeliveryAddress
            try
            {
                _unitOfWork.Repository<DeliveryAddress>().Delete(deliveryAddress);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[DeliveryAddressService.DeleteDeliveryAddress()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<DeliveryAddress>
                    {
                        ResultCode = (int)CommonResponse.ERROR,
                        ResultMessage = CommonResponse.ERROR.ToString(),
                        Data = default
                    });
            }

            //Create Response
            DeliveryAddressResponse deliveryAddressResponse = _mapper.Map<DeliveryAddressResponse>(deliveryAddress);

            return new BaseResponse<DeliveryAddressResponse>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = deliveryAddressResponse
            };

        }


        /// <summary>
        /// Get DeliveryAddress By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<BaseResponse<DeliveryAddressResponse>> GetDeliveryAddressById(string id)
        {
            //biz rule


            DeliveryAddressResponse deliveryAddressResponse;

            //Get DeliveryAddress From Database

                try
                {
                    DeliveryAddress deliveryAddress = await _unitOfWork.Repository<DeliveryAddress>().
                                                            FindAsync(local => local.DeliveryAddressId.Equals(id));

                    deliveryAddressResponse = _mapper.Map<DeliveryAddressResponse>(deliveryAddress);
                }
                catch (Exception e)
                {
                    _logger.Error("[DeliveryAddressService.GetDeliveryAddressById()]: " + e.Message);

                    throw new HttpStatusException(HttpStatusCode.OK,
                        new BaseResponse<DeliveryAddressResponse>
                        {
                            ResultCode = (int)DeliveryAddressStatus.DELIVERYADDRESS_NOT_FOUND,
                            ResultMessage = DeliveryAddressStatus.DELIVERYADDRESS_NOT_FOUND.ToString(),
                            Data = default
                        });
                }

            return new BaseResponse<DeliveryAddressResponse>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = deliveryAddressResponse
            };
        }


        /// <summary>
        /// Update DeliveryAddress
        /// </summary>
        /// <param name="id"></param>
        /// <param name="DeliveryAddressRequest"></param>
        /// <returns></returns>
        public async Task<BaseResponse<DeliveryAddressResponse>> UpdateDeliveryAddressById(string id, DeliveryAddressRequest deliveryAddressRequest)
        {
            //biz ruie

            //Check id
            DeliveryAddress deliveryAddress;
            try
            {
                deliveryAddress = await _unitOfWork.Repository<DeliveryAddress>()
                                       .FindAsync(local => local.DeliveryAddressId.Equals(id));
            }
            catch (Exception e)
            {
                _logger.Error("[DeliveryAddressService.UpdateDeliveryAddress()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK, new BaseResponse<DeliveryAddressResponse>
                {
                    ResultCode = (int)DeliveryAddressStatus.DELIVERYADDRESS_NOT_FOUND,
                    ResultMessage = DeliveryAddressStatus.DELIVERYADDRESS_NOT_FOUND.ToString(),
                    Data = default
                });
            }

            //Update DeliveryAddress To DB
            try
            {
                deliveryAddress = _mapper.Map(deliveryAddressRequest, deliveryAddress);

                _unitOfWork.Repository<DeliveryAddress>().Update(deliveryAddress);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[DeliveryAddressService.UpdateDeliveryAddress()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK, new BaseResponse<DeliveryAddressResponse>
                {
                    ResultCode = (int)CommonResponse.ERROR,
                    ResultMessage = CommonResponse.ERROR.ToString(),
                    Data = default
                });
            }

            //Create Response
            DeliveryAddressResponse DeliveryAddressResponse = _mapper.Map<DeliveryAddressResponse>(deliveryAddress);

            return new BaseResponse<DeliveryAddressResponse>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = DeliveryAddressResponse
            };
        }
    }
}
