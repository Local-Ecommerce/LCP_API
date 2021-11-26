using AutoMapper;
using BLL.Dtos;
using BLL.Dtos.Exception;
using BLL.Dtos.Merchant;
using BLL.Services.Interfaces;
using DAL.Models;
using DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class MerchantService : IMerchantService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IRedisService _redisService;
        private readonly IUtilService _utilService;
        private const string CACHE_KEY = "Merchant";

        public MerchantService(IUnitOfWork unitOfWork,
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
        /// Create Merchant
        /// </summary>
        /// <param name="merchantRequest"></param>
        /// <returns></returns>
        public async Task<BaseResponse<MerchantResponse>> CreateMerchant(MerchantRequest merchantRequest)
        {

            //biz rule

            //Store Merchant To Database
            Merchant merchant = _mapper.Map<Merchant>(merchantRequest);
            try
            {
                merchant.MerchantId = _utilService.Create16Alphanumeric();
                merchant.IsBlock = false;
                merchant.IsActive = false;
                merchant.LevelId = "1";

                _unitOfWork.Repository<Merchant>().Add(merchant);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[MerchantService.CreateMerchant()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<MerchantResponse>
                    {
                        ResultCode = (int)MerchantStatus.ERROR,
                        ResultMessage = MerchantStatus.ERROR.ToString(),
                        Data = default
                    });
            }

            //Create response
            MerchantResponse merchantResponse = _mapper.Map<MerchantResponse>(merchant);

            //Store Merchant To Redis
            _redisService.StoreToList(CACHE_KEY, merchantResponse,
                    new Predicate<MerchantResponse>(a => a.MerchantId == merchantResponse.MerchantId));

            return new BaseResponse<MerchantResponse>
            {
                ResultCode = (int)MerchantStatus.SUCCESS,
                ResultMessage = MerchantStatus.SUCCESS.ToString(),
                Data = merchantResponse
            };
        }


        /// <summary>
        /// Get Merchant By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<BaseResponse<MerchantResponse>> GetMerchantById(string id)
        {

            //biz rule

            MerchantResponse merchantResponse = null;

            //Get Merchant From Redis
            merchantResponse = _redisService.GetList<MerchantResponse>(CACHE_KEY)
                .Find(merchant => merchant.MerchantId.Equals(id));

            if(merchantResponse is null)
            {
                //Get Merchant From Database
                try
                {
                    Merchant merchant = await _unitOfWork.Repository<Merchant>()
                                                       .FindAsync(m => m.MerchantId.Equals(id));
                    merchantResponse = _mapper.Map<MerchantResponse>(merchant);
                }
                catch(Exception e)
                {
                    _logger.Error("[MerchantService.GetMerchantById()]: " + e.Message);

                    throw new HttpStatusException(HttpStatusCode.OK,
                        new BaseResponse<MerchantResponse>
                        {
                            ResultCode = (int)MerchantStatus.MERCHANT_NOT_FOUND,
                            ResultMessage = MerchantStatus.MERCHANT_NOT_FOUND.ToString(),
                            Data = default
                        });
                }
            }

            return new BaseResponse<MerchantResponse>
            {
                ResultCode = (int)MerchantStatus.SUCCESS,
                ResultMessage = MerchantStatus.SUCCESS.ToString(),
                Data = merchantResponse
            };

        }


        /// <summary>
        /// Update Merchant By Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="merchantRequest"></param>
        /// <returns></returns>
        public async Task<BaseResponse<MerchantResponse>> UpdateMerchantById(string id, MerchantRequest merchantRequest)
        {

            Merchant merchant;

            //Check id
            try
            {
                merchant = await _unitOfWork.Repository<Merchant>().
                                             FindAsync(m => m.MerchantId.Equals(id));
            }
            catch(Exception e)
            {
                _logger.Error("[MerchantService.UpdateMerchantById()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<MerchantResponse>
                    {
                        ResultCode = (int)MerchantStatus.MERCHANT_NOT_FOUND,
                        ResultMessage = MerchantStatus.MERCHANT_NOT_FOUND.ToString(),
                        Data = default
                    });
            }

            //update merchant
            try
            {
                merchant = _mapper.Map(merchantRequest, merchant);

                _unitOfWork.Repository<Merchant>().Update(merchant);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[MerchantService.UpdateMerchantById()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<MerchantResponse>
                    {
                        ResultCode = (int)MerchantStatus.ERROR,
                        ResultMessage = MerchantStatus.ERROR.ToString(),
                        Data = default
                    });
            }

            //Create Response
            MerchantResponse merchantResponse = _mapper.Map<MerchantResponse>(merchant);

            //Store Merchant To Redis
            _redisService.StoreToList(CACHE_KEY, merchantResponse,
                    new Predicate<MerchantResponse>(a => a.MerchantId == merchantResponse.MerchantId));

            return new BaseResponse<MerchantResponse>
            {
                ResultCode = (int)MerchantStatus.SUCCESS,
                ResultMessage = MerchantStatus.SUCCESS.ToString(),
                Data = merchantResponse
            };
        }


        /// <summary>
        /// Delete Merchant
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<BaseResponse<MerchantResponse>> DeleteMerchant(string id)
        {

            //biz rule

            //Check id
            Merchant merchant;
            try
            {
                merchant = await _unitOfWork.Repository<Merchant>().
                                                      FindAsync(m => m.MerchantId.Equals(id));
            } 
            catch (Exception e)
            {
                _logger.Error("[MerchantService.DeleteMerchant()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<MerchantResponse>
                    {
                        ResultCode = (int)MerchantStatus.MERCHANT_NOT_FOUND,
                        ResultMessage = MerchantStatus.MERCHANT_NOT_FOUND.ToString(),
                        Data = default
                    });
            }

            //Delete Merchant
            try
            {
                merchant.IsBlock = true;

                _unitOfWork.Repository<Merchant>().Update(merchant);

                await _unitOfWork.SaveChangesAsync();
            }
            catch(Exception e)
            {
                _logger.Error("[MerchantService.DeleteMerchant()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<MerchantResponse>
                    {
                        ResultCode = (int)MerchantStatus.ERROR,
                        ResultMessage = MerchantStatus.ERROR.ToString(),
                        Data = default
                    });
            }

            //Create Response
            MerchantResponse merchantResponse = _mapper.Map<MerchantResponse>(merchant);

            //Store Merchant To Redis
            _redisService.StoreToList(CACHE_KEY, merchantResponse,
                    new Predicate<MerchantResponse>(a => a.MerchantId == merchantResponse.MerchantId));

            return new BaseResponse<MerchantResponse>
            {
                ResultCode = (int)MerchantStatus.SUCCESS,
                ResultMessage = MerchantStatus.SUCCESS.ToString(),
                Data = merchantResponse
            };
        }
    }
}
