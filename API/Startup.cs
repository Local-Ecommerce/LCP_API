using API.Extensions;
using BLL.Dtos;
using DAL.Constants;
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
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json;
using System;
using System.Reflection;
using System.IO;
using API.Middleware;

namespace API
{
    public class Startup
    {
        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
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
                    builder.WithOrigins(Configuration.GetValue<string>("CorsOrigin:Backend"), 
                        Configuration.GetValue<string>("CorsOrigin:Frontend"), Configuration.GetValue<string>("CorsOrigin:Server"))
                           .AllowAnyHeader()
                           .AllowAnyMethod();
                });
            });

            //Add Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Local Commerce Platform", Version = "v1.9" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            //Add Redis
            services.AddStackExchangeRedisCache(opt =>
            {
                opt.Configuration = Configuration.GetValue<string>
                ("CacheSettings:ConnectionString");
            });

            //Add DB connection
            services.AddDbContext<LoichDBContext>(opt => opt.UseSqlServer(
                Configuration.GetConnectionString("DatabaseConnection")));


            //Add Logger
            services.AddSingleton<ILogger, LogNLog>();

            //Add Error Handler
            services.AddMvc(options =>
            {
                options.Filters.Add(new ErrorHandlingFilter());
            });

            //Add JWT Authentication
            var key = Configuration.GetValue<string>("Jwt:Custom:Key");

            //Custom Jwt Authentication
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
                        string response = JsonSerializer.Serialize(
                            ApiResponse<string>.Fail((int)AccountStatus.UNAUTHORIZED_ACCOUNT, AccountStatus.UNAUTHORIZED_ACCOUNT.ToString()));
                        await context.Response.WriteAsync(response);
                    }
                };
            });

            services.AddSingleton<IJwtAuthenticationManager>(
                new JwtAuthenticationManager(key));

            //add middleware
            services.AddTransient<CheckBlacklistTokenMiddleware>();

            //add application service extensions
            services.AddApplicationServices(Configuration);

            //setting environment
            string startupPath = System.IO.Directory.GetCurrentDirectory();
            string credential_path = startupPath + "\\firebase_auth.json";
            System.Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", credential_path);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.RoutePrefix = "";
                    c.SwaggerEndpoint("/swagger/v1.9/swagger.json", "LCP v1.9");
                });
            //}

            app.UseHttpsRedirection();

            app.UseRouting();

            //add CORS
            app.UseCors("MyPolicy");

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseMiddleware<CheckBlacklistTokenMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
