using AutoMapper;
using BLL.Dtos;
using BLL.Dtos.Customer;
using BLL.Dtos.Exception;
using BLL.Services.Interfaces;
using DAL.Models;
using DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static BLL.Dtos.Customer.CustomerRequest;

namespace BLL.Services
{
    public class CustomerService : ICustomerService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IRedisService _redisService;
        private readonly IUtilService _utilService;
        private const string CACHE_KEY = "Customer";

        public CustomerService(IUnitOfWork unitOfWork,
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
        /// Create Customer
        /// </summary>
        /// <param name="customerRequest"></param>
        /// <returns></returns>
        public async Task<BaseResponse<CustomerResponse>> CreateCustomer(CustomerRequest customerRequest)
        {
            //biz rule

            //Store Customer To Dabatabase
            Customer customer = _mapper.Map<Customer>(customerRequest);

            try
            {
                customer.CustomerId = _utilService.Create16Alphanumeric();
                customer.IsActive = true;
                customer.CreatedDate = DateTime.Now;
                customer.UpdatedDate = DateTime.Now;

                _unitOfWork.Repository<Customer>().Add(customer);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[CustomerService.CreateCustomer()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<CustomerResponse>
                    {
                        ResultCode = (int)CustomerStatus.ERROR,
                        ResultMessage = CustomerStatus.ERROR.ToString(),
                        Data = default
                    });
            }

            //Create Response
            CustomerResponse CustomerResponse = _mapper.Map<CustomerResponse>(customer);

            //Store Customer To Redis
            _redisService.StoreToList(CACHE_KEY, CustomerResponse,
                    new Predicate<CustomerResponse>(a => a.CustomerId == CustomerResponse.CustomerId));

            return new BaseResponse<CustomerResponse>
            {
                ResultCode = (int)CustomerStatus.SUCCESS,
                ResultMessage = CustomerStatus.SUCCESS.ToString(),
                Data = CustomerResponse
            };

        }


        /// <summary>
        /// Delete Customer
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<BaseResponse<CustomerResponse>> DeleteCustomer(string id)
        {
            //biz rule

            //Check id
            Customer customer;
            try
            {
                customer = await _unitOfWork.Repository<Customer>()
                                       .FindAsync(local => local.CustomerId.Equals(id));
            }
            catch (Exception e)
            {
                _logger.Error("[CustomerService.DeleteCustomer()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<Customer>
                    {
                        ResultCode = (int)CustomerStatus.CUSTOMER_NOT_FOUND,
                        ResultMessage = CustomerStatus.CUSTOMER_NOT_FOUND.ToString(),
                        Data = default
                    });
            }

            //Delete Customer
            try
            {
                customer.IsBlock = false;

                _unitOfWork.Repository<Customer>().Update(customer);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[CustomerService.DeleteCustomer()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<Customer>
                    {
                        ResultCode = (int)CustomerStatus.ERROR,
                        ResultMessage = CustomerStatus.ERROR.ToString(),
                        Data = default
                    });
            }

            //Create Response
            CustomerResponse customerResponse = _mapper.Map<CustomerResponse>(customer);

            //Store Customer To Redis
            _redisService.StoreToList(CACHE_KEY, customerResponse,
                    new Predicate<CustomerResponse>(a => a.CustomerId == customerResponse.CustomerId));

            return new BaseResponse<CustomerResponse>
            {
                ResultCode = (int)CustomerStatus.SUCCESS,
                ResultMessage = CustomerStatus.SUCCESS.ToString(),
                Data = customerResponse
            };
        }


        /// <summary>
        /// Get Customer By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<BaseResponse<CustomerResponse>> GetCustomerById(string id)
        {
            //biz rule


            CustomerResponse customerResponse = null;
            //Get Customer From Redis
            customerResponse = _redisService.GetList<CustomerResponse>(CACHE_KEY)
                                            .Find(local => local.CustomerId.Equals(id));

            //Get Customer From Database
            if (customerResponse is null)
            {
                try
                {
                    Customer customer = await _unitOfWork.Repository<Customer>().
                                                            FindAsync(local => local.CustomerId.Equals(id));

                    customerResponse = _mapper.Map<CustomerResponse>(customer);
                }
                catch (Exception e)
                {
                    _logger.Error("[CustomerService.GetCustomerById()]: " + e.Message);

                    throw new HttpStatusException(HttpStatusCode.OK,
                        new BaseResponse<CustomerResponse>
                        {
                            ResultCode = (int)CustomerStatus.CUSTOMER_NOT_FOUND,
                            ResultMessage = CustomerStatus.CUSTOMER_NOT_FOUND.ToString(),
                            Data = default
                        });
                }
            }

            return new BaseResponse<CustomerResponse>
            {
                ResultCode = (int)CustomerStatus.SUCCESS,
                ResultMessage = CustomerStatus.SUCCESS.ToString(),
                Data = customerResponse
            };
        }


        /// <summary>
        /// Update Customer By Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="customerRequest"></param>
        /// <returns></returns>
        public async Task<BaseResponse<CustomerResponse>> UpdateCustomerById(string id, CustomerRequest customerRequest)
        {
            //biz ruie

            //Check id
            Customer customer;
            try
            {
                customer = await _unitOfWork.Repository<Customer>()
                                       .FindAsync(local => local.CustomerId.Equals(id));
            }
            catch (Exception e)
            {
                _logger.Error("[CustomerService.UpdateCustomerById()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK, new BaseResponse<CustomerResponse>
                {
                    ResultCode = (int)CustomerStatus.CUSTOMER_NOT_FOUND,
                    ResultMessage = CustomerStatus.CUSTOMER_NOT_FOUND.ToString(),
                    Data = default
                });
            }

            //Update Customer To DB
            try
            {
                customer = _mapper.Map(customerRequest, customer);

                _unitOfWork.Repository<Customer>().Update(customer);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[CustomerService.UpdateCustomerById()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK, new BaseResponse<CustomerResponse>
                {
                    ResultCode = (int)CustomerStatus.ERROR,
                    ResultMessage = CustomerStatus.ERROR.ToString(),
                    Data = default
                });
            }

            //Create Response
            CustomerResponse customerResponse = _mapper.Map<CustomerResponse>(customer);

            //Store Reponse To Redis
            _redisService.StoreToList(CACHE_KEY, customerResponse,
                    new Predicate<CustomerResponse>(a => a.CustomerId == customerResponse.CustomerId));

            return new BaseResponse<CustomerResponse>
            {
                ResultCode = (int)CustomerStatus.SUCCESS,
                ResultMessage = CustomerStatus.SUCCESS.ToString(),
                Data = customerResponse
            };
        }
    }
}
