using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.Events;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Persistence;
using Persistence.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Application.Services;

namespace API
{
	public class Startup
	{
		private readonly IConfiguration _config;

		public Startup(IConfiguration config)
		{
			_config = config;
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllers();

			// Repositories
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
						.UseSqlServer(_config
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
						IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetValue<string>("Authentication:JWTSecretKey"))),
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
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseSwagger();
				app
					.UseSwaggerUI(c =>
						c
							.SwaggerEndpoint("/swagger/v1/swagger.json",
							"WebAPIv5 v1"));
			}

			// app.UseHttpsRedirection();
			app.UseCors("CorsPolicy");

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app
				.UseEndpoints(endpoints =>
				{
					endpoints.MapControllers();
				});
		}
	}
}
