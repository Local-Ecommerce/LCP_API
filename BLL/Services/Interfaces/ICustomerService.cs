/*using BLL.Dtos;
using BLL.Dtos.Customer;
using System;
using System.Collections.Generic;
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


        /// <summary>
        /// Get Customer By Name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<BaseResponse<CustomerResponse>> GetCustomerByName(string name);


        /// <summary>
        /// Get Customer By Phone
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        Task<BaseResponse<CustomerResponse>> GetCustomerByPhoneNumber(string phone);


        /// <summary>
        /// Get Customer By Date Of Birth
        /// </summary>
        /// <param name="dob"></param>
        /// <returns></returns>
        Task<BaseResponse<CustomerResponse>> GetCustomerByDateOfBirth(DateTime dob);


        /// <summary>
        /// Get Customer By Gender
        /// </summary>
        /// <param name="gender"></param>
        /// <returns></returns>
        Task<BaseResponse<List<CustomerResponse>>> GetCustomersByGender(string gender);


        /// <summary>
        /// Get Customer By Create Date
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        Task<BaseResponse<List<CustomerResponse>>> GetCustomersByCreatedDate(DateTime date);


        /// <summary>
        /// Get Customer By Update Date
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        Task<BaseResponse<List<CustomerResponse>>> GetCustomersByUpdateDate(DateTime date);
    }
}
*/