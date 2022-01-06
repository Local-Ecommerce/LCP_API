using AutoMapper;
using BLL.Dtos;
using BLL.Constants;
using BLL.Dtos.Customer;
using BLL.Dtos.Exception;
using BLL.Services.Interfaces;
using DAL.Models;
using DAL.UnitOfWork;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BLL.Services
{
    public class CustomerService : ICustomerService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IUtilService _utilService;
        private const string PREFIX = "CTM_";

        public CustomerService(IUnitOfWork unitOfWork,
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
                customer.CustomerId = _utilService.CreateId(PREFIX);
                customer.CreatedDate = DateTime.Now;
                customer.UpdatedDate = DateTime.Now;
                customer.Status = (int)CustomerStatus.UNVERIFIED_CREATE_CUSTOMER;

                _unitOfWork.Repository<Customer>().Add(customer);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[CustomerService.CreateCustomer()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<CustomerResponse>
                    {
                        ResultCode = (int)CommonResponse.ERROR,
                        ResultMessage = CommonResponse.ERROR.ToString(),
                        Data = default
                    });
            }

            //Create Response
            CustomerResponse CustomerResponse = _mapper.Map<CustomerResponse>(customer);

            return new BaseResponse<CustomerResponse>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
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
                                       .FindAsync(cus => cus.CustomerId.Equals(id));
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
                customer.Status = (int)CustomerStatus.DELETED_CUSTOMER;

                _unitOfWork.Repository<Customer>().Update(customer);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[CustomerService.DeleteCustomer()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<Customer>
                    {
                        ResultCode = (int)CommonResponse.ERROR,
                        ResultMessage = CommonResponse.ERROR.ToString(),
                        Data = default
                    });
            }

            //Create Response
            CustomerResponse customerResponse = _mapper.Map<CustomerResponse>(customer);

            return new BaseResponse<CustomerResponse>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
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


            CustomerResponse customerResponse;

            //Get Customer From Database

                try
                {
                    Customer customer = await _unitOfWork.Repository<Customer>().
                                                            FindAsync(cus => cus.CustomerId.Equals(id));

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

            return new BaseResponse<CustomerResponse>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
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
                                       .FindAsync(cus => cus.CustomerId.Equals(id));
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
                customer.Status = (int)CustomerStatus.UNVERIFIED_UPDATE_CUSTOMER;
                customer.UpdatedDate = DateTime.Now;

                _unitOfWork.Repository<Customer>().Update(customer);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[CustomerService.UpdateCustomerById()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK, new BaseResponse<CustomerResponse>
                {
                    ResultCode = (int)CommonResponse.ERROR,
                    ResultMessage = CommonResponse.ERROR.ToString(),
                    Data = default
                });
            }

            //Create Response
            CustomerResponse customerResponse = _mapper.Map<CustomerResponse>(customer);

            return new BaseResponse<CustomerResponse>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = customerResponse
            };
        }


        /// <summary>
        /// Get Customer By Name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="HttpStatusException"></exception>
        public async Task<BaseResponse<CustomerResponse>> GetCustomerByName(string name)
        {
            //biz rule


            CustomerResponse customerResponse;

            //Get Customer From Database

            try
            {
                Customer customer = await _unitOfWork.Repository<Customer>().
                                                        FindAsync(cus => cus.CustomerName.Equals(name));

                customerResponse = _mapper.Map<CustomerResponse>(customer);
            }
            catch (Exception e)
            {
                _logger.Error("[CustomerService.GetCustomerByName()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<CustomerResponse>
                    {
                        ResultCode = (int)CustomerStatus.CUSTOMER_NOT_FOUND,
                        ResultMessage = CustomerStatus.CUSTOMER_NOT_FOUND.ToString(),
                        Data = default
                    });
            }

            return new BaseResponse<CustomerResponse>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = customerResponse
            };
        }


        /// <summary>
        /// Get Customer By Phone Number
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        /// <exception cref="HttpStatusException"></exception>
        public async Task<BaseResponse<CustomerResponse>> GetCustomerByPhoneNumber(string phone)
        {
            //biz rule


            CustomerResponse customerResponse;

            //Get Customer From Database

            try
            {
                Customer customer = await _unitOfWork.Repository<Customer>().
                                                        FindAsync(cus => cus.PhoneNumber.Equals(phone));

                customerResponse = _mapper.Map<CustomerResponse>(customer);
            }
            catch (Exception e)
            {
                _logger.Error("[CustomerService.GetCustomerByPhoneNumber()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<CustomerResponse>
                    {
                        ResultCode = (int)CustomerStatus.CUSTOMER_NOT_FOUND,
                        ResultMessage = CustomerStatus.CUSTOMER_NOT_FOUND.ToString(),
                        Data = default
                    });
            }

            return new BaseResponse<CustomerResponse>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = customerResponse
            };
        }


        /// <summary>
        /// Get Customer By Date Of Birth
        /// </summary>
        /// <param name="dob"></param>
        /// <returns></returns>
        /// <exception cref="HttpStatusException"></exception>
        public async Task<BaseResponse<CustomerResponse>> GetCustomerByDateOfBirth(DateTime dob)
        {
            //biz rule


            CustomerResponse customerResponse;

            //Get Customer From Database

            try
            {
                Customer customer = await _unitOfWork.Repository<Customer>().
                                                        FindAsync(cus => cus.DateOfBirth.Value.Date == dob.Date);

                customerResponse = _mapper.Map<CustomerResponse>(customer);
            }
            catch (Exception e)
            {
                _logger.Error("[CustomerService.GetCustomerByPhoneNumber()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<CustomerResponse>
                    {
                        ResultCode = (int)CustomerStatus.CUSTOMER_NOT_FOUND,
                        ResultMessage = CustomerStatus.CUSTOMER_NOT_FOUND.ToString(),
                        Data = default
                    });
            }

            return new BaseResponse<CustomerResponse>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = customerResponse
            };
        }


        /// <summary>
        /// Get Customers By Gender
        /// </summary>
        /// <param name="gender"></param>
        /// <returns></returns>
        /// <exception cref="HttpStatusException"></exception>
        public async Task<BaseResponse<List<CustomerResponse>>> GetCustomersByGender(string gender)
        {
            //biz rule


            List<CustomerResponse> customerResponses;

            //Get Customer From Database

            try
            {
                List<Customer> customers = await _unitOfWork.Repository<Customer>().
                                                        FindListAsync(cus => cus.Gender.Equals(gender));

                customerResponses = _mapper.Map<List<CustomerResponse>>(customers);
            }
            catch (Exception e)
            {
                _logger.Error("[CustomerService.GetCustomerByGender()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<CustomerResponse>
                    {
                        ResultCode = (int)CustomerStatus.CUSTOMER_NOT_FOUND,
                        ResultMessage = CustomerStatus.CUSTOMER_NOT_FOUND.ToString(),
                        Data = default
                    });
            }

            return new BaseResponse<List<CustomerResponse>>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = customerResponses
            };
        }


        /// <summary>
        /// Get Customers By Created Date
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        /// <exception cref="HttpStatusException"></exception>
        public async Task<BaseResponse<List<CustomerResponse>>> GetCustomersByCreatedDate(DateTime date)
        {
            //biz rule


            List<CustomerResponse> customerResponses;

            //Get Customer From Database

            try
            {
                List<Customer> customers = await _unitOfWork.Repository<Customer>().
                                                        FindListAsync(cus => cus.CreatedDate.Value.Date == date.Date);

                customerResponses = _mapper.Map<List<CustomerResponse>>(customers);
            }
            catch (Exception e)
            {
                _logger.Error("[CustomerService.GetCustomerByCreatedDate()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<CustomerResponse>
                    {
                        ResultCode = (int)CustomerStatus.CUSTOMER_NOT_FOUND,
                        ResultMessage = CustomerStatus.CUSTOMER_NOT_FOUND.ToString(),
                        Data = default
                    });
            }

            return new BaseResponse<List<CustomerResponse>>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = customerResponses
            };
        }


        /// <summary>
        /// Get Customer By Update Date
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        /// <exception cref="HttpStatusException"></exception>
        public async Task<BaseResponse<List<CustomerResponse>>> GetCustomersByUpdateDate(DateTime date)
        {
            //biz rule


            List<CustomerResponse> customerResponses;

            //Get Customer From Database

            try
            {
                List<Customer> customers = await _unitOfWork.Repository<Customer>().
                                                        FindListAsync(cus => cus.UpdatedDate.Value.Date == date.Date);

                customerResponses = _mapper.Map<List<CustomerResponse>>(customers);
            }
            catch (Exception e)
            {
                _logger.Error("[CustomerService.GetCustomerByUpdateDate()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<CustomerResponse>
                    {
                        ResultCode = (int)CustomerStatus.CUSTOMER_NOT_FOUND,
                        ResultMessage = CustomerStatus.CUSTOMER_NOT_FOUND.ToString(),
                        Data = default
                    });
            }

            return new BaseResponse<List<CustomerResponse>>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = customerResponses
            };
        }


        /// <summary>
        /// Get Customer By Account Id
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        /// <exception cref="HttpStatusException"></exception>
        public async Task<BaseResponse<List<CustomerResponse>>> GetCustomersByAccountId(string accountId)
        {
            //biz rule


            List<CustomerResponse> customerResponses;

            //Get Customer From Database

            try
            {
                List<Customer> customers = await _unitOfWork.Repository<Customer>().
                                                        FindListAsync(cus => cus.AccountId.Equals(accountId));

                customerResponses = _mapper.Map<List<CustomerResponse>>(customers);
            }
            catch (Exception e)
            {
                _logger.Error("[CustomerService.GetCustomerByAccountId()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<CustomerResponse>
                    {
                        ResultCode = (int)CustomerStatus.CUSTOMER_NOT_FOUND,
                        ResultMessage = CustomerStatus.CUSTOMER_NOT_FOUND.ToString(),
                        Data = default
                    });
            }

            return new BaseResponse<List<CustomerResponse>>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = customerResponses
            };
        }
    }
}
