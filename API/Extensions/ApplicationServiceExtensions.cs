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
using EFCoreSecondLevelCacheInterceptor;
using MessagePack.Formatters;
using MessagePack.Resolvers;
using MessagePack;
using API.SignalR;
using Microsoft.Extensions.DependencyInjection;

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

			services.AddScoped<UserFCMTokenRepository>();
			services.AddScoped<UserFCMTokenService>();

			services.AddScoped<EventCodeRepository>();
			services.AddScoped<EventCodeService>();

			services.AddScoped<LocationRepository>();
			services.AddScoped<LocationService>();

			services.AddScoped<TokenService>();

			services.AddSingleton<FirebaseService>();
			// services.AddSingleton<AWSService>();
			services.AddSingleton<GCService>();
			services.AddSingleton<RedisConnection>();


			services.AddSignalR();
			services.AddSingleton<PostConnections>();

			services.AddHostedService<BackgroundEventCheckService>();
			services.AddHostedService<RedisQueueBackgroundService>();


			services.AddScoped<IUserAccessor, UserAccessor>();
			services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

			// Swagger
			services
			  .AddSwaggerGen(c =>
			  {
				  //   c.EnableAnnotations();
				  c.OrderActionsBy((apiDesc) => $"{apiDesc.RelativePath}");
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
									.EnableRetryOnFailure();
					})
					.AddInterceptors(prov.GetRequiredService<SecondLevelCacheInterceptor>())
					.EnableThreadSafetyChecks(false);
			  });

			// Add Redis Cache
			const string _providerName = "redis1";
			services.AddEFSecondLevelCache(options =>
			   {
				   options.UseEasyCachingCoreProvider(_providerName, isHybridCache: false).DisableLogging(false)
					//   .CacheAllQueries(CacheExpirationMode.Sliding, TimeSpan.FromDays(1));
					// .SkipCachingCommands(commandText =>
					// 	commandText.Contains("NEWID()", StringComparison.InvariantCultureIgnoreCase)); ;
					.CacheAllQueries(CacheExpirationMode.Absolute, TimeSpan.FromDays(7));
			   });

			services.AddEasyCaching(option =>
			 {
				 option
				 	.UseRedis(configuration: config, _providerName, sectionName: "easycaching:redis")
				 	.WithMessagePack(so =>
					{
						so.EnableCustomResolver = true;
						so.CustomResolvers = CompositeResolver.Create(
							new IMessagePackFormatter[]
							{
								DBNullFormatter.Instance // This is necessary for the null values
							},
							new IFormatterResolver[]
							{
								NativeDateTimeResolver.Instance,
								ContractlessStandardResolver.Instance,
								StandardResolverAllowPrivate.Instance,
								TypelessContractlessStandardResolver.Instance,
							}
						);
					},
					"_msgpack");
			 });


			services.AddMediatR(typeof(List.Handler).Assembly);

			services.AddAutoMapper(typeof(MappingProfiles).Assembly);

			return services;
		}
	}

	public class DBNullFormatter : IMessagePackFormatter<DBNull>
	{
		public static DBNullFormatter Instance = new();

		private DBNullFormatter()
		{
		}

		public void Serialize(ref MessagePackWriter writer, DBNull value, MessagePackSerializerOptions options)
		{
			writer.WriteNil();
		}

		public DBNull Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
		{
			return DBNull.Value;
		}
	}
}