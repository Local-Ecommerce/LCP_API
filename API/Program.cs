using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using System.IO;
using System.Reflection;
using System;
using BLL.Services;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using BLL.Services.Interfaces;
using API.Extensions;
using BLL.Filters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json;
using BLL.Dtos;
using DAL.Constants;
using Microsoft.AspNetCore.Http;

var builder = WebApplication.CreateBuilder(args);
var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

#region Services
var services = builder.Services;

//add Controllers
services.AddControllers();

//add CORS
services.AddCors(options => {
	options.AddPolicy(name: "MyPolicy", builder => {
		builder.WithOrigins(
				configuration.GetValue<string>("CorsOrigin:Backend"),
				configuration.GetValue<string>("CorsOrigin:Frontend"),
				configuration.GetValue<string>("CorsOrigin:Admin"),
				configuration.GetValue<string>("CorsOrigin:Admin2"),
				configuration.GetValue<string>("CorsOrigin:Merchant"),
				configuration.GetValue<string>("CorsOrigin:Merchant2")
		)
		.AllowAnyHeader()
		.AllowAnyMethod();
	});
});

//Add Swagger
services.AddSwaggerGen(c => {
	c.SwaggerDoc("v3.2", new OpenApiInfo { Title = "Local Commerce Platform", Version = "v3.2" });
	c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
		Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')",
		Name = "Authorization",
		In = ParameterLocation.Header,
		Type = SecuritySchemeType.ApiKey,
		Scheme = "Bearer"
	});

	c.AddSecurityRequirement(new OpenApiSecurityRequirement {
		{
			new OpenApiSecurityScheme {
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
services.AddStackExchangeRedisCache(opt => {
	opt.Configuration = configuration.GetValue<string>
	("CacheSettings:ConnectionString");
});

//Add DB connection
services.AddDbContext<LoichDBContext>(opt => opt.UseSqlServer(
		configuration.GetConnectionString("DatabaseConnection")));

//Add Logger
services.AddSingleton<ILogger, LogNLog>();

//Add Error Handler
services.AddMvc(options => {
	options.Filters.Add(new ErrorHandlingFilter());
});

//Custom Jwt Authentication
var key = Encoding.ASCII.GetBytes(configuration.GetValue<string>("Jwt:Custom:Key"));
var tokenValidationParameters = new TokenValidationParameters {
	ValidateIssuerSigningKey = true,
	IssuerSigningKey = new SymmetricSecurityKey(key),
	ValidateIssuer = false,
	ValidateAudience = false,
	ValidateLifetime = false,
	RequireExpirationTime = false,
	ClockSkew = TimeSpan.Zero
};

//Add JWT Authentication
services.AddAuthentication(x => {
	x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
	x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x => {
	x.SaveToken = true;
	x.TokenValidationParameters = tokenValidationParameters;

	x.Events = new JwtBearerEvents {
		OnChallenge = async context => {
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

services.AddSingleton<ITokenService>(new TokenService(key, tokenValidationParameters));

//add application service extensions
services.AddApplicationServices(configuration);

//setting environment
string startupPath = Directory.GetCurrentDirectory();
string credential_path = startupPath + "\\firebase_auth.json";
Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", credential_path);
#endregion

#region Application
var app = builder.Build();

app.UseDeveloperExceptionPage();
app.UseSwagger();
app.UseSwaggerUI(c => {
	c.RoutePrefix = "";
	c.SwaggerEndpoint("/swagger/v3.2/swagger.json", "LCP v3.2");
});

app.UseHttpsRedirection();

app.UseRouting();

//add CORS
app.UseCors("MyPolicy");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
#endregion
