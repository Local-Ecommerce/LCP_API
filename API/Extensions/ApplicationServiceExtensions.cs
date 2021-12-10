using BLL.Services;
using BLL.Services.Interfaces;
using DAL.UnitOfWork;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {

        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            //Add Mapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            //Add Unit Of Work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            //Add Logger
            services.AddSingleton<ILogger, LogNLog>();

            //Add service
            services.AddScoped<IRedisService, RedisService>();
            services.AddScoped<IValidateDataService, ValidateDataService>();
            services.AddScoped<IUtilService, UtilService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IMerchantService, MerchantService>();
            services.AddScoped<IApartmentService, ApartmentService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ISystemCategoryService, SystemCategoryService>();
            services.AddScoped<IProductCategoryService, ProductCategoryService>();
            services.AddScoped<IApartmentService, ApartmentService>();
            services.AddScoped<IMerchantStoreService, MerchantStoreService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<ICollectionService, CollectionService>();

            return services;
        }
    }
}
