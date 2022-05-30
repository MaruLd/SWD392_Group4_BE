using Application.Core;
using Application.Events;
using Application.Interfaces;
using Application.Services;
using Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Persistence;
using Persistence.Repositories;
using Application.Services;

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

			services.AddScoped<FirebaseService>();

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

			


			services.AddMediatR(typeof(List.Handler).Assembly);
			services.AddAutoMapper(typeof(MappingProfiles).Assembly);
			services.AddScoped<IUserAccessor, UserAccessor>();
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