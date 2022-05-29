using System.Collections.Generic;
using System.Text;
using Application.Core;
using Application.Events;
using Application.Services;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Persistence;
using Persistence.Repositories;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config){
            services.AddScoped<TicketRepository>();
			services.AddScoped<EventRepository>();

			services.AddScoped<TicketService>();
			services.AddScoped<EventService>();
			services.AddScoped<TokenService>();

			// Swagger
			services
				.AddSwaggerGen(c =>
				{
					c
						.SwaggerDoc("v1",
						new OpenApiInfo { Title = "WebAPIv5", Version = "v1" });
				});

			// DataContext
			services
				.AddDbContext<DataContext>(opt =>
				{
					opt
						.UseSqlServer(config
							.GetConnectionString("DefaultConnection"));
				});

			services.AddIdentityCore<User>()
				.AddEntityFrameworkStores<DataContext>()
				.AddDefaultTokenProviders();

			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer(options =>
				{
					options.TokenValidationParameters = new TokenValidationParameters
					{
						ValidateIssuerSigningKey = true,
						IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetValue<string>("Authentication:JWTSecretKey"))),
						ValidateIssuer = false,
						ValidateAudience = false,
					};
				});


			services.AddMediatR(typeof(List.Handler).Assembly);
			services.AddAutoMapper(typeof(MappingProfiles).Assembly);
			services
				.AddCors(opt =>
				{
					opt
						.AddPolicy("CorsPolicy",
						policy =>
						{
							policy
								.AllowAnyMethod()
								.AllowAnyHeader()
								.WithOrigins("http://localhost:3000");
						});
				});
                return services;
        }
    }
}