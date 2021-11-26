using BLL.Dtos;
using BLL.Dtos.Merchant;
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

    }
}
