using BLL.Dtos;
using BLL.Dtos.MarketManager;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BLL.Services.Interfaces
{
    public interface IMarketManagerService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="managerRequest"></param>
        /// <returns></returns>
        Task<BaseResponse<MarketManagerResponse>> CreateMarketManager(MarketManagerRequest managerRequest);


        /// <summary>
        /// Get MarketManager By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BaseResponse<MarketManagerResponse>> GetMarketManagerById(string id);


        /// <summary>
        /// Update MarketManager By Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="MarketManagerRequest"></param>
        /// <returns></returns>
        Task<BaseResponse<MarketManagerResponse>> UpdateMarketManagerById(string id, MarketManagerRequest MarketManagerRequest);


        /// <summary>
        /// Delete MarketManager
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BaseResponse<MarketManagerResponse>> DeleteMarketManager(string id);

        /// <summary>
        /// Get MarketManager By Account Id
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        Task<BaseResponse<List<MarketManagerResponse>>> GetMarketManagerByAccountId(string accountId);


        /// <summary>
        /// Get Market Manager By Status
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        Task<BaseResponse<List<MarketManagerResponse>>> GetMarketManagerByStatus(int status);
    }
}
