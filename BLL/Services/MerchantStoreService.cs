﻿using AutoMapper;
using BLL.Dtos;
using BLL.Constants;
using BLL.Dtos.Exception;
using BLL.Dtos.MerchantStore;
using BLL.Services.Interfaces;
using DAL.Models;
using DAL.UnitOfWork;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using BLL.Dtos.StoreMenuDetail;
using System.Linq;
using BLL.Dtos.Merchant;
using BLL.Dtos.Apartment;

namespace BLL.Services
{
    public class MerchantStoreService : IMerchantStoreService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IUtilService _utilService;
        private const string PREFIX = "MS_";
        private const string SUB_PREFIX = "SMD_";


        public MerchantStoreService(IUnitOfWork unitOfWork,
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
        /// Create Merchant Store
        /// </summary>
        /// <param name="merchantStoreRequest"></param>
        /// <returns></returns>
        public async Task<BaseResponse<MerchantStoreResponse>> CreateMerchantStore(MerchantStoreRequest merchantStoreRequest)
        {
            //biz rule

            //Store MerchantStore To Database
            MerchantStore merchantStore = _mapper.Map<MerchantStore>(merchantStoreRequest);
            try
            {
                merchantStore.MerchantStoreId = _utilService.CreateId(PREFIX);
                merchantStore.Status = (int)MerchantStoreStatus.UNVERIFIED_CREATE_MERCHANT_STORE;
                merchantStore.CreatedDate = DateTime.Now;

                _unitOfWork.Repository<MerchantStore>().Add(merchantStore);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[MerchantStoreService.CreateMerchantStore()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<MerchantStoreResponse>
                    {
                        ResultCode = (int)CommonResponse.ERROR,
                        ResultMessage = CommonResponse.ERROR.ToString(),
                        Data = default
                    });
            }

            //Create response
            MerchantStoreResponse merchantStoreResponse = _mapper.Map<MerchantStoreResponse>(merchantStore);

            return new BaseResponse<MerchantStoreResponse>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = merchantStoreResponse
            };
        }


        /// <summary>
        /// Delete Merchant Store
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<BaseResponse<MerchantStoreResponse>> DeleteMerchantStore(string id)
        {
            //biz rule

            //Check id
            MerchantStore merchantStore;
            try
            {
                merchantStore = await _unitOfWork.Repository<MerchantStore>().
                                                      FindAsync(m => m.MerchantStoreId.Equals(id));
            }
            catch (Exception e)
            {
                _logger.Error("[MerchantStoreService.DeleteMerchantStore()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<MerchantStoreResponse>
                    {
                        ResultCode = (int)MerchantStoreStatus.MERCHANT_STORE_NOT_FOUND,
                        ResultMessage = MerchantStoreStatus.MERCHANT_STORE_NOT_FOUND.ToString(),
                        Data = default
                    });
            }

            //Delete MerchantStore
            try
            {
                merchantStore.Status = (int)MerchantStoreStatus.DELETED_MERCHANT_STORE;

                _unitOfWork.Repository<MerchantStore>().Update(merchantStore);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[MerchantStoreService.DeleteMerchantStore()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<MerchantStoreResponse>
                    {
                        ResultCode = (int)CommonResponse.ERROR,
                        ResultMessage = CommonResponse.ERROR.ToString(),
                        Data = default
                    });
            }

            //Create Response
            MerchantStoreResponse merchantStoreResponse = _mapper.Map<MerchantStoreResponse>(merchantStore);

            return new BaseResponse<MerchantStoreResponse>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = merchantStoreResponse
            };
        }


        /// <summary>
        /// Get Merchant Store By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<BaseResponse<MerchantStoreResponse>> GetMerchantStoreById(string id)
        {
            //biz rule

            MerchantStoreResponse merchantStoreResponse;

            //Get MerchantStore From Database
            try
            {
                MerchantStore merchantStore = await _unitOfWork.Repository<MerchantStore>()
                                                   .FindAsync(m => m.MerchantStoreId.Equals(id));
                merchantStoreResponse = _mapper.Map<MerchantStoreResponse>(merchantStore);
            }
            catch (Exception e)
            {
                _logger.Error("[MerchantStoreService.GetMerchantById()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<MerchantStoreResponse>
                    {
                        ResultCode = (int)MerchantStoreStatus.MERCHANT_STORE_NOT_FOUND,
                        ResultMessage = MerchantStoreStatus.MERCHANT_STORE_NOT_FOUND.ToString(),
                        Data = default
                    });
            }

            return new BaseResponse<MerchantStoreResponse>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = merchantStoreResponse
            };
        }


        /// <summary>
        /// Update Merchant Store By Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="merchantStoreRequest"></param>
        /// <returns></returns>
        public async Task<BaseResponse<MerchantStoreResponse>> UpdateMerchantStoreById(string id,
            MerchantStoreRequest merchantStoreRequest)
        {
            MerchantStore merchantStore;

            //Check id
            try
            {
                merchantStore = await _unitOfWork.Repository<MerchantStore>().
                                             FindAsync(m => m.MerchantStoreId.Equals(id));
            }
            catch (Exception e)
            {
                _logger.Error("[MerchantStoreService.UpdateMerchantStoreById()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<MerchantStoreResponse>
                    {
                        ResultCode = (int)MerchantStoreStatus.MERCHANT_STORE_NOT_FOUND,
                        ResultMessage = MerchantStoreStatus.MERCHANT_STORE_NOT_FOUND.ToString(),
                        Data = default
                    });
            }

            //update MerchantStore
            try
            {
                merchantStore = _mapper.Map(merchantStoreRequest, merchantStore);
                merchantStore.Status = (int)MerchantStoreStatus.UNVERIFIED_UPDATE_MERCHANT_STORE;

                _unitOfWork.Repository<MerchantStore>().Update(merchantStore);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[MerchantStoreService.UpdateMerchantById()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<MerchantStoreResponse>
                    {
                        ResultCode = (int)CommonResponse.ERROR,
                        ResultMessage = CommonResponse.ERROR.ToString(),
                        Data = default
                    });
            }

            //Create Response
            MerchantStoreResponse merchantStoreResponse = _mapper.Map<MerchantStoreResponse>(merchantStore);

            return new BaseResponse<MerchantStoreResponse>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = merchantStoreResponse
            };
        }


        /// <summary>
        /// Get Merchant Store By Store Name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<BaseResponse<MerchantStoreResponse>> GetMerchantStoreByStoreName(string name)
        {
            //biz rule

            MerchantStoreResponse merchantStoreResponse;

            //Get MerchantStore From Database

            try
            {
                MerchantStore merchantStore = await _unitOfWork.Repository<MerchantStore>()
                                                   .FindAsync(m => m.StoreName.Equals(name));
                merchantStoreResponse = _mapper.Map<MerchantStoreResponse>(merchantStore);
            }
            catch (Exception e)
            {
                _logger.Error("[MerchantStoreService.GetMerchantStoreByStoreName()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<MerchantStoreResponse>
                    {
                        ResultCode = (int)MerchantStoreStatus.MERCHANT_STORE_NOT_FOUND,
                        ResultMessage = MerchantStoreStatus.MERCHANT_STORE_NOT_FOUND.ToString(),
                        Data = default
                    });
            }

            return new BaseResponse<MerchantStoreResponse>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = merchantStoreResponse
            };
        }


        /// <summary>
        /// Get Merchant Store By Merchant Id
        /// </summary>
        /// <param name="merchantId"></param>
        /// <returns></returns>
        public async Task<BaseResponse<List<MerchantStoreResponse>>> GetMerchantStoreByMerchantId(string merchantId)
        {
            List<MerchantStoreResponse> merchantStoreResponses;

            //Get MerchantStore From Database

            try
            {
                List<MerchantStore> merchantStores = await _unitOfWork.Repository<MerchantStore>().
                                                        FindListAsync
                                                        (store => store.MerchantId.Equals(merchantId));

                merchantStoreResponses = _mapper.Map<List<MerchantStoreResponse>>(merchantStores);
            }
            catch (Exception e)
            {
                _logger.Error("[MerchantStoreService.GetMerchantStoreByMerchantId()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<MerchantStoreResponse>
                    {
                        ResultCode = (int)MerchantStatus.MERCHANT_NOT_FOUND,
                        ResultMessage = MerchantStatus.MERCHANT_NOT_FOUND.ToString(),
                        Data = default
                    });
            }

            return new BaseResponse<List<MerchantStoreResponse>>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = merchantStoreResponses
            };
        }


        /// <summary>
        /// Get Merchant Store By Apartment Id
        /// </summary>
        /// <param name="apartmentId"></param>
        /// <returns></returns>
        public async Task<BaseResponse<List<MerchantStoreResponse>>> GetMerchantStoreByApartmentId(string apartmentId)
        {
            List<MerchantStoreResponse> merchantStoreResponses;

            //Get MerchantStore From Database

            try
            {
                List<MerchantStore> merchantStores = await _unitOfWork.Repository<MerchantStore>().
                                                        FindListAsync
                                                        (store => store.ApartmentId.Equals(apartmentId));

                merchantStoreResponses = _mapper.Map<List<MerchantStoreResponse>>(merchantStores);
            }
            catch (Exception e)
            {
                _logger.Error("[MerchantStoreService.GetMerchantStoreByAppartmentId()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<MerchantStoreResponse>
                    {
                        ResultCode = (int)MerchantStatus.MERCHANT_NOT_FOUND,
                        ResultMessage = MerchantStatus.MERCHANT_NOT_FOUND.ToString(),
                        Data = default
                    });
            }

            return new BaseResponse<List<MerchantStoreResponse>>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = merchantStoreResponses
            };
        }


        /// <summary>
        /// Add Store Menu Details To Merchant Store
        /// </summary>
        /// <param name="merchantStoreId"></param>
        /// <param name="storeMenuDetailRequest"></param>
        /// <returns></returns>
        public async Task<BaseResponse<List<StoreMenuDetailResponse>>> AddStoreMenuDetailsToMerchantStore(string merchantStoreId,
            List<StoreMenuDetailRequest> storeMenuDetailRequest)
        {
            List<StoreMenuDetail> storeMenuDetails = _mapper.Map<List<StoreMenuDetail>>(storeMenuDetailRequest);
            try
            {
                storeMenuDetails.ForEach(storeMenuDetail =>
                {
                    storeMenuDetail.StoreMenuDetailId = _utilService.CreateId(SUB_PREFIX);
                    storeMenuDetail.CreatedDate = DateTime.Now;
                    storeMenuDetail.MerchantStoreId = merchantStoreId;
                    storeMenuDetail.Status = (int)StoreMenuDetailStatus.ACTIVE_STORE_MENU_DETAIL;

                    _unitOfWork.Repository<StoreMenuDetail>().Add(storeMenuDetail);

                });

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[MerchantStoreService.AddStoreMenuDetailsToMerchantStore()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<StoreMenuDetailResponse>
                    {
                        ResultCode = (int)CommonResponse.ERROR,
                        ResultMessage = CommonResponse.ERROR.ToString(),
                        Data = default
                    });
            }

            //Create response
            List<StoreMenuDetailResponse> storeMenuDetailResponses = _mapper.Map<List<StoreMenuDetailResponse>>(storeMenuDetails);

            return new BaseResponse<List<StoreMenuDetailResponse>>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = storeMenuDetailResponses
            };
        }


        /// <summary>
        /// Get Store Menu Details By Merchant Store Id
        /// </summary>
        /// <param name="merchantStoreId"></param>
        /// <returns></returns>
        public async Task<BaseResponse<List<StoreMenuDetailResponse>>> GetStoreMenuDetailsByMerchantStoreId(string merchantStoreId)
        {
            List<StoreMenuDetail> storeMenuDetails;
            try
            {
                storeMenuDetails = await _unitOfWork.Repository<StoreMenuDetail>()
                    .FindListAsync(smd => smd.MerchantStoreId == merchantStoreId);
            }
            catch (Exception e)
            {
                _logger.Error("[MerchantStoreService.GetStoreMenuDetailsByMerchantStoreId()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<StoreMenuDetailResponse>
                    {
                        ResultCode = (int)StoreMenuDetailStatus.STORE_MENU_DETAIL_NOT_FOUND,
                        ResultMessage = StoreMenuDetailStatus.STORE_MENU_DETAIL_NOT_FOUND.ToString(),
                        Data = default
                    });
            }

            //Create response
            List<StoreMenuDetailResponse> storeMenuDetailResponses = _mapper.Map<List<StoreMenuDetailResponse>>(storeMenuDetails);

            return new BaseResponse<List<StoreMenuDetailResponse>>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = storeMenuDetailResponses
            };
        }


        /// <summary>
        /// Get Store Menu Detail By Id
        /// </summary>
        /// <param name="storeMenuDetailId"></param>
        /// <returns></returns>
        public async Task<BaseResponse<StoreMenuDetailResponse>> GetStoreMenuDetailById(string storeMenuDetailId)
        {
            StoreMenuDetail storeMenuDetail;
            try
            {
                storeMenuDetail = await _unitOfWork.Repository<StoreMenuDetail>()
                    .FindAsync(smd => smd.StoreMenuDetailId == storeMenuDetailId);
            }
            catch (Exception e)
            {
                _logger.Error("[MerchantStoreService.GetStoreMenuDetailById()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<StoreMenuDetailResponse>
                    {
                        ResultCode = (int)StoreMenuDetailStatus.STORE_MENU_DETAIL_NOT_FOUND,
                        ResultMessage = StoreMenuDetailStatus.STORE_MENU_DETAIL_NOT_FOUND.ToString(),
                        Data = default
                    });
            }

            //Create response
            StoreMenuDetailResponse storeMenuDetailResponse = _mapper.Map<StoreMenuDetailResponse>(storeMenuDetail);

            return new BaseResponse<StoreMenuDetailResponse>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = storeMenuDetailResponse
            };
        }


        /// <summary>
        /// Update Store Menu Detail By Id
        /// </summary>
        /// <param name="storeMenuDetailId"></param>
        /// <param name="storeMenuDetailUpdateRequest"></param>
        /// <returns></returns>
        public async Task<BaseResponse<StoreMenuDetailResponse>> UpdateStoreMenuDetailById(string storeMenuDetailId,
            StoreMenuDetailUpdateRequest storeMenuDetailUpdateRequest)
        {
            StoreMenuDetail storeMenuDetail;
            try
            {
                storeMenuDetail = await _unitOfWork.Repository<StoreMenuDetail>()
                    .FindAsync(smd => smd.StoreMenuDetailId == storeMenuDetailId);

                storeMenuDetail.Status = storeMenuDetailUpdateRequest.Status;
                storeMenuDetail.TimeStart = storeMenuDetailUpdateRequest.TimeStart;
                storeMenuDetail.TimeEnd = storeMenuDetailUpdateRequest.TimeEnd;

                _unitOfWork.Repository<StoreMenuDetail>().Update(storeMenuDetail);
            }
            catch (Exception e)
            {
                _logger.Error("[MerchantStoreService.UpdateStoreMenuDetailById()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<StoreMenuDetailResponse>
                    {
                        ResultCode = (int)CommonResponse.ERROR,
                        ResultMessage = CommonResponse.ERROR.ToString(),
                        Data = default
                    });
            }

            //Create response
            StoreMenuDetailResponse storeMenuDetailResponse = _mapper.Map<StoreMenuDetailResponse>(storeMenuDetail);

            return new BaseResponse<StoreMenuDetailResponse>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = storeMenuDetailResponse
            };
        }


        /// <summary>
        /// Delete Store Menu Detail By Id
        /// </summary>
        /// <param name="storeMenuDetailId"></param>
        /// <returns></returns>
        public async Task<BaseResponse<StoreMenuDetailResponse>> DeleteStoreMenuDetailById(string storeMenuDetailId)
        {
            StoreMenuDetail storeMenuDetail;
            try
            {
                storeMenuDetail = await _unitOfWork.Repository<StoreMenuDetail>()
                    .FindAsync(smd => smd.StoreMenuDetailId == storeMenuDetailId);

                storeMenuDetail.Status = (int)StoreMenuDetailStatus.DELETED_STORE_MENU_DETAIL;

                _unitOfWork.Repository<StoreMenuDetail>().Update(storeMenuDetail);
            }
            catch (Exception e)
            {
                _logger.Error("[MerchantStoreService.DeleteStoreMenuDetailById()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<StoreMenuDetailResponse>
                    {
                        ResultCode = (int)CommonResponse.ERROR,
                        ResultMessage = CommonResponse.ERROR.ToString(),
                        Data = default
                    });
            }

            //Create response
            StoreMenuDetailResponse storeMenuDetailResponse = _mapper.Map<StoreMenuDetailResponse>(storeMenuDetail);

            return new BaseResponse<StoreMenuDetailResponse>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = storeMenuDetailResponse
            };
        }


        /// <summary>
        /// Get Merchant Stores By Status
        /// </summary>
        /// <returns></returns>
        /// <exception cref="HttpStatusException"></exception>
        public async Task<BaseResponse<List<MerchantStoreResponse>>> GetMerchantStoresByStatus(int status)
        {
            List<MerchantStoreResponse> merchantStoreList = null;

            //get merchantStore from database
            try
            {
                merchantStoreList = _mapper.Map<List<MerchantStoreResponse>>(
                    await _unitOfWork.Repository<MerchantStore>()
                                     .FindListAsync(ms => ms.Status == status));
            }
            catch (Exception e)
            {
                _logger.Error("[MerchantStoreService.GetMerchantStoresByStatus()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<MerchantStoreResponse>
                    {
                        ResultCode = (int)MerchantStoreStatus.MERCHANT_STORE_NOT_FOUND,
                        ResultMessage = MerchantStoreStatus.MERCHANT_STORE_NOT_FOUND.ToString(),
                        Data = default
                    });
            }

            return new BaseResponse<List<MerchantStoreResponse>>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = merchantStoreList
            };
        }


        /// <summary>
        /// Get All Merchant Stores
        /// </summary>
        /// <returns></returns>
        /// <exception cref="HttpStatusException"></exception>
        public async Task<BaseResponse<List<MerchantStoreResponse>>> GetAllMerchantStores()
        {
            List<MerchantStoreResponse> merchantStoreList;

            //Get MerchantStore from database

            try
            {
                await using var context = new LoichDBContext();

                merchantStoreList = (from mcs in context.MerchantStores
                                     join mc in context.Merchants
                                     on mcs.MerchantId equals mc.MerchantId
                                     join ap in context.Apartments
                                     on mcs.ApartmentId equals ap.ApartmentId
                                     orderby mcs.CreatedDate descending
                                     select new MerchantStoreResponse
                                     {
                                         MerchantStoreId = mcs.MerchantStoreId,
                                         ApartmentId = mcs.ApartmentId,
                                         CreatedDate = mcs.CreatedDate,
                                         MerchantId = mcs.MerchantId,
                                         Status = mcs.Status,
                                         StoreName = mcs.StoreName,
                                         Merchant = new MerchantResponse
                                         {
                                             AccountId = mc.AccountId,
                                             Address = mc.Address,
                                             ApproveBy = mc.ApproveBy,
                                             LevelId = mc.LevelId,
                                             MerchantId = mc.MerchantId,
                                             MerchantName = mc.MerchantName,
                                             PhoneNumber = mc.PhoneNumber,
                                             Status = mc.Status
                                         },
                                         Apartment = new ApartmentResponse
                                         {
                                             ApartmentId = ap.ApartmentId,
                                             Address = ap.Address,
                                             Status = ap.Status,
                                             Lat = ap.Lat,
                                             Long = ap.Long,
                                         }
                                     }).ToList();

            }
            catch (Exception e)
            {
                _logger.Error("[MerchantStoreService.GetAllMerchantStores()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<MerchantStoreResponse>
                    {
                        ResultCode = (int)MerchantStoreStatus.MERCHANT_STORE_NOT_FOUND,
                        ResultMessage = MerchantStoreStatus.MERCHANT_STORE_NOT_FOUND.ToString(),
                        Data = default
                    });
            }

            return new BaseResponse<List<MerchantStoreResponse>>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = merchantStoreList
            };
        }
    }
}
