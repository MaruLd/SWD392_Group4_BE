using Application.Core;
using Application.Events;
using Application.Interfaces;
using Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Persistence.Repositories;
using Application.Services;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Domain;
using Persistence;
using System.Reflection;

namespace API.Extensions
{
	public static class ApplicationServiceExtensions
	{
		public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
		{
			services.AddScoped<EventRepository>();
			services.AddScoped<EventAgenda>();
			services.AddScoped<EventUserRepository>();
			services.AddScoped<EventCategory>();
			services.AddScoped<UserRepository>();
			services.AddScoped<TicketRepository>();
			services.AddScoped<PostRepository>();
			services.AddScoped<CommentRepository>();
			services.AddScoped<OrganizerRepository>();
			services.AddScoped<EventCategoryRepository>();
			services.AddScoped<EventAgendaRepository>();
			services.AddScoped<CommentRepository>();

			services.AddScoped<EventService>();
			services.AddScoped<TicketService>();
			services.AddScoped<PostService>();
			services.AddScoped<UserService>();
			services.AddScoped<EventUserService>();
			services.AddScoped<OrganizerService>();
			services.AddScoped<EventCategoryService>();
			services.AddScoped<EventAgendaService>();
			services.AddScoped<CommentService>();

			services.AddScoped<TokenService>();
			services.AddSingleton<FirebaseService>();
			services.AddSingleton<AWSService>();

			services.AddScoped<UserAccessor>();
			services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();


			// Swagger
			services
			  .AddSwaggerGen(c =>
			  {
				  c.SwaggerDoc("v1",
		  new OpenApiInfo { Title = "WebAPIv5", Version = "v1" });
				  var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
				  var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
				  c.IncludeXmlComments(xmlPath);
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
	  .WithOrigins("http://localhost:3000",
	  "https://evsmart.netlify.app",
	  "http://evsmart.netlify.app");
		  });
			  });

			services.AddSignalR();
			return services;
		}
	}
}