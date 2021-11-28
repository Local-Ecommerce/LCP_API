using BLL.Dtos;
using BLL.Dtos.Customer;
using System.Threading.Tasks;

namespace BLL.Services.Interfaces
{
    public interface ICustomerService
    {


        /// <summary>
        /// Create Customer
        /// </summary>
        /// <param name="customerRequest"></param>
        /// <returns></returns>
        Task<BaseResponse<CustomerResponse>> CreateCustomer(CustomerRequest customerRequest);


        /// <summary>
        /// Get Customer By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BaseResponse<CustomerResponse>> GetCustomerById(string id);


        /// <summary>
        /// Update Customer By Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="customerRequest"></param>
        /// <returns></returns>
        Task<BaseResponse<CustomerResponse>> UpdateCustomerById(string id, CustomerRequest customerRequest);


        /// <summary>
        /// Delete Customer
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BaseResponse<CustomerResponse>> DeleteCustomer(string id);
    }
}
