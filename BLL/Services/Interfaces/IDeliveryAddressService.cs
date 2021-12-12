using BLL.Dtos;
using BLL.Dtos.DeliveryAddress;
using System.Threading.Tasks;

namespace BLL.Services.Interfaces
{
    public interface IDeliveryAddressService
    {
        /// <summary>
        /// Create DeliveryAddress
        /// </summary>
        /// <param name="deliveryAddressRequest"></param>
        /// <returns></returns>
        Task<BaseResponse<DeliveryAddressResponse>> CreateDeliveryAddress(DeliveryAddressRequest deliveryAddressRequest);


        /// <summary>
        /// Get DeliveryAddress By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BaseResponse<DeliveryAddressResponse>> GetDeliveryAddressById(string id);


        /// <summary>
        /// Update DeliveryAddress By Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="deliveryAddressRequest"></param>
        /// <returns></returns>
        Task<BaseResponse<DeliveryAddressResponse>> UpdateDeliveryAddressById(string id, DeliveryAddressRequest deliveryAddressRequest);


        /// <summary>
        /// Delete DeliveryAddress
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BaseResponse<DeliveryAddressResponse>> DeleteDeliveryAddress(string id);
    }
}
