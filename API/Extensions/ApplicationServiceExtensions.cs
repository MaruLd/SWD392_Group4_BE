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
using Application.Events.StateMachine;
using EFCoreSecondLevelCacheInterceptor;
using EasyCaching.Core.Configurations;

namespace API.Extensions
{
	public static class ApplicationServiceExtensions
	{
		public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
		{
			services.AddScoped<EventRepository>();
			services.AddScoped<EventService>();

			services.AddScoped<EventAgendaRepository>();
			services.AddScoped<EventAgendaService>();

			services.AddScoped<EventUserRepository>();
			services.AddScoped<EventUserService>();

			services.AddScoped<EventCategory>();
			services.AddScoped<EventCategoryService>();

			services.AddScoped<UserRepository>();
			services.AddScoped<UserService>();

			services.AddScoped<TicketRepository>();
			services.AddScoped<TicketService>();

			services.AddScoped<PostRepository>();
			services.AddScoped<PostService>();

			services.AddScoped<CommentRepository>();
			services.AddScoped<CommentService>();

			services.AddScoped<OrganizerRepository>();
			services.AddScoped<OrganizerService>();

			services.AddScoped<EventCategoryRepository>();
			services.AddScoped<EventCategoryService>();

			services.AddScoped<TicketUserRepository>();
			services.AddScoped<TicketUserService>();

			services.AddScoped<EventOrganizerRepository>();
			services.AddScoped<EventOrganizerService>();

			services.AddScoped<UserImageRepository>();
			services.AddScoped<UserImageService>();

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
				  c.IncludeXmlComments(xmlPath, true);

				  var securitySchema = new OpenApiSecurityScheme
				  {
					  Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
					  Name = "Authorization",
					  In = ParameterLocation.Header,
					  Type = SecuritySchemeType.Http,
					  Scheme = "bearer",
					  Reference = new OpenApiReference
					  {
						  Type = ReferenceType.SecurityScheme,
						  Id = "Bearer"
					  }
				  };

				  c.AddSecurityDefinition("Bearer", securitySchema);

				  var securityRequirement = new OpenApiSecurityRequirement
				{
					{ securitySchema, new[] { "Bearer" } }
				};

				  c.AddSecurityRequirement(securityRequirement);
			  });


			// DataContext
			services
			  .AddDbContextPool<DataContext>((prov, opt) =>
			  {
				  opt
					.UseSqlServer(config
					.GetConnectionString("DefaultConnection"),
					sqlOpts =>
					{
						sqlOpts.CommandTimeout((int)TimeSpan.FromMinutes(3).TotalSeconds)
									.EnableRetryOnFailure()
									.MigrationsAssembly(typeof(ApplicationServiceExtensions).Assembly.FullName);
					})
					.AddInterceptors(prov.GetRequiredService<SecondLevelCacheInterceptor>());
			  });

			// Add Redis Cache
			const string _providerName = "redis1";
			services.AddEFSecondLevelCache(options =>
			   {
				   options.UseEasyCachingCoreProvider(_providerName, isHybridCache: false)
				   		.SkipCachingCommands(commandText =>
								commandText.Contains("NEWID()", StringComparison.InvariantCultureIgnoreCase)); ;
				   options.CacheQueriesContainingTableNames(CacheExpirationMode.Absolute, TimeSpan.FromMinutes(30), TableNameComparison.Contains, "Event");

			   });

			services.AddEasyCaching(option =>
			 {
				 option.UseRedis(configuration: config, _providerName, sectionName: "easycaching:redis").WithMessagePack("_msgpack");
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