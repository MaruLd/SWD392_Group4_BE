using System.Text;
using Domain;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Persistence;

namespace API.Extensions
{
	public static class IdentityServicesExtensions
	{
		public static IServiceCollection
		AddIdentityServices(
			this IServiceCollection services,
			IConfiguration config
		)
		{
			services
				.AddIdentity<User, IdentityRole<Guid>>()
				.AddEntityFrameworkStores<DataContext>()
				.AddDefaultTokenProviders();

			services
				.AddAuthentication(options =>
				{
					options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
					options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
				})
				.AddJwtBearer(options =>
				{
					options.TokenValidationParameters =
						new TokenValidationParameters
						{
							ValidateIssuerSigningKey = true,
							IssuerSigningKey =
								new SymmetricSecurityKey(Encoding
										.UTF8
										.GetBytes(config
											.GetValue
											<string
											>("Authentication:JWTSecretKey"))),
							ValidateIssuer = false,
							ValidateAudience = false
						};
					options.Events = new JwtBearerEvents
					{
						OnMessageReceived = context =>
						{
							var accessToken = context.Request.Query["access_token"];
							var path = context.HttpContext.Request.Path;
							if (!string.IsNullOrEmpty(accessToken) && (path.StartsWithSegments("/chat")))
							{
								context.Token = accessToken;
							}
							return Task.CompletedTask;
						}
					};
				});

			return services;
		}
	}
}
