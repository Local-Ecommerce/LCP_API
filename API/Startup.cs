using API.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

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

            //add application service extensions
            services.AddApplicationServices(_configuration);

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
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Local_4_You", Version = "v1" });
            });

            //Add Redis
            services.AddStackExchangeRedisCache(opt =>
            {
                opt.Configuration = _configuration.GetValue<string>
                ("CacheSettings:ConnectionString");
            });


        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Local_4_You v1"));
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
