using BLL.Services;
using BLL.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RepositoryLayer.UnitOfWork;
using System;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {

        /*
         * [12/08/2021 - HanNQ] app application services
         */
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


            return services;
        }
    }
}
