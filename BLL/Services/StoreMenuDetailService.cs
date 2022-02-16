using System;
using System.Threading.Tasks;
using AutoMapper;
using DAL.Constants;
using BLL.Dtos.StoreMenuDetail;
using BLL.Services.Interfaces;
using DAL.Models;
using DAL.UnitOfWork;

namespace BLL.Services
{
    public class StoreMenuDetailService : IStoreMenuDetailService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IUtilService _utilService;
        private const string PREFIX = "SMD_";


        public StoreMenuDetailService(IUnitOfWork unitOfWork,
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
        /// Create Default Store Menu Detail
        /// </summary>
        /// <param name="menuId"></param>
        /// <param name="merchantStoreId"></param>
        /// <returns></returns>
        public StoreMenuDetailResponse CreateDefaultStoreMenuDetail(string menuId, string merchantStoreId)
        {

            StoreMenuDetail storeMenuDetail = new()
            {
                StoreMenuDetailId = _utilService.CreateId(PREFIX),
                TimeStart = TimeSpan.Zero,
                TimeEnd = TimeSpan.FromHours((int)TimeUnit.TWENTY_FOUR_HOUR),
                Status = (int)StoreMenuDetailStatus.ACTIVE_STORE_MENU_DETAIL,
                RepeatDate = "2345678",
                MenuId = menuId,
                MerchantStoreId = merchantStoreId
            };

            _unitOfWork.StoreMenuDetails.Add(storeMenuDetail);

            return _mapper.Map<StoreMenuDetailResponse>(storeMenuDetail);
        }


        /// <summary>
        /// Create Store Menu Detail
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<StoreMenuDetailResponse> CreateStoreMenuDetail(StoreMenuDetailRequest request)
        {
            StoreMenuDetail storeMenuDetail;

            try
            {
                storeMenuDetail = _mapper.Map<StoreMenuDetail>(request);
                storeMenuDetail.StoreMenuDetailId = _utilService.CreateId(PREFIX);

                _unitOfWork.StoreMenuDetails.Add(storeMenuDetail);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[StoreMenuDetailService.CreateStoreMenuDetail()]: " + e.Message);

                throw;
            }

            return _mapper.Map<StoreMenuDetailResponse>(storeMenuDetail);
        }


        /// <summary>
        /// Update Store Menu Detail By Id
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<StoreMenuDetailResponse> UpdateStoreMenuDetailById(string id, StoreMenuDetailUpdateRequest request)
        {
            StoreMenuDetail storeMenuDetail;

            //get Store Menu Detail By Id
            try
            {
                storeMenuDetail = await _unitOfWork.StoreMenuDetails.FindAsync(smd => smd.StoreMenuDetailId.Equals(id));

                storeMenuDetail = _mapper.Map<StoreMenuDetail>(request);

                _unitOfWork.StoreMenuDetails.Update(storeMenuDetail);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[StoreMenuDetailService.UpdateStoreMenuDetailById()]: " + e.Message);

                throw;
            }

            return _mapper.Map<StoreMenuDetailResponse>(storeMenuDetail);
        }


        /// <summary>
        /// Delete Store Menu Detail By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<StoreMenuDetailResponse> DeleteStoreMenuDetailById(string id)
        {
            StoreMenuDetail storeMenuDetail;

            //get store menu detail by id
            try
            {
                storeMenuDetail = await _unitOfWork.StoreMenuDetails.FindAsync(smd => smd.StoreMenuDetailId.Equals(id));

                storeMenuDetail.Status = (int)StoreMenuDetailStatus.DELETED_STORE_MENU_DETAIL;

                _unitOfWork.StoreMenuDetails.Update(storeMenuDetail);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[StoreMenuDetailService.DeleteStoreMenuDetailById()]: " + e.Message);

                throw;
            }

            return _mapper.Map<StoreMenuDetailResponse>(storeMenuDetail);
        }
    }
}