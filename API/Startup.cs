using API.Extensions;
using BLL.Dtos;
using BLL.Constants;
using BLL.Filters;
using BLL.Services;
using BLL.Services.Interfaces;
using DAL.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json;

namespace API
{
    public class Startup
    {
        private IConfiguration _configuration { get; }

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            //add Controllers
            services.AddControllers();

            //add CORS
            services.AddCors(options =>
            {
                options.AddPolicy(name: "MyPolicy", builder =>
                {
                    builder.WithOrigins(_configuration.GetValue<string>("ServerLink"))
                           .AllowAnyHeader()
                           .AllowAnyMethod(); ;
                });
            });

            //Add Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Local Commerce Platform", Version = "v1" });
            });

            //Add Redis
            services.AddStackExchangeRedisCache(opt =>
            {
                opt.Configuration = _configuration.GetValue<string>
                ("CacheSettings:ConnectionString");
            });

            //Add DB connection
            services.AddDbContext<LoichDBContext>(opt => opt.UseSqlServer(
                _configuration.GetConnectionString("DatabaseConnection")));

            //Add Error Handler
            services.AddMvc(options =>
            {
                options.Filters.Add(new ErrorHandlingFilter());
            });

            //Add JWT Authentication
            var key = _configuration.GetValue<string>("SecretKey");

            //Jwt Authentication
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };

                x.Events = new JwtBearerEvents
                {
                    OnChallenge = async context =>
                    {
                        // Call this to skip the default logic and avoid using the default response
                        context.HandleResponse();

                        // Write to the response
                        context.Response.StatusCode = 401;
                        BaseResponse<string> response = new BaseResponse<string>
                        {
                            ResultCode = (int)AccountStatus.UNAUTHORIZED_ACCOUNT,
                            ResultMessage = AccountStatus.UNAUTHORIZED_ACCOUNT.ToString()
                        };
                        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                    }
                };
            });

            services.AddSingleton<IJwtAuthenticationManager>(
                new JwtAuthenticationManager(key));

            //add application service extensions
            services.AddApplicationServices(_configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "LCP v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            //add CORS
            app.UseCors("MyPolicy");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
