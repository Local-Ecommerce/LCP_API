/*using BLL.Dtos;
using BLL.Dtos.Merchant;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Services.Interfaces
{
    public interface IMerchantService
    {
        /// <summary>
        /// Create Merchant
        /// </summary>
        /// <param name="merchantRequest"></param>
        /// <returns></returns>
        Task<BaseResponse<MerchantResponse>> CreateMerchant(MerchantRequest merchantRequest);


        /// <summary>
        /// Update Merchant By Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="merchantRequest"></param>
        /// <returns></returns>
        Task<BaseResponse<MerchantResponse>> UpdateMerchantById(string id, MerchantRequest merchantRequest);


        /// <summary>
        /// Get Merchant By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BaseResponse<MerchantResponse>> GetMerchantById(string id);


        /// <summary>
        /// Delete Merchant
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BaseResponse<MerchantResponse>> DeleteMerchant(string id);


        /// <summary>
        /// Get Merchant By Name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<BaseResponse<MerchantResponse>> GetMerchantByName(string name);


        /// <summary>
        /// Get Merchant By Address
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        Task<BaseResponse<MerchantResponse>> GetMerchantByAddress(string address);


        /// <summary>
        /// Get Merchant By Phone Number
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        Task<BaseResponse<MerchantResponse>> GetMerchantByPhoneNumber(string number);


        /// <summary>
        /// Get Merchant By Account Id
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        Task<BaseResponse<MerchantResponse>> GetMerchantByAccountId(string accountId);


        /// <summary>
        /// Get Merchants By Status
        /// </summary>
        /// <returns></returns>
        Task<BaseResponse<List<MerchantResponse>>> GetMerchantsByStatus(int status);
    }
}
*/