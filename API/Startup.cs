

using System.Text.Json;
using System.Text.Json.Serialization;
using API.Extensions;
using API.Middleware;
using API.SignalR;
using JorgeSerrano.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;

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
			services.AddRouting(options =>
			{
				options.LowercaseUrls = true;
				options.LowercaseQueryStrings = true;
			});

			services.AddControllers()
			.AddJsonOptions(opts =>
			{
				opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
				opts.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;

				opts.JsonSerializerOptions.PropertyNamingPolicy = new JsonKebabCaseNamingPolicy();
				opts.JsonSerializerOptions.DictionaryKeyPolicy = new JsonKebabCaseNamingPolicy();
				opts.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
			});

			services.AddApiVersioning(o =>
			{
				o.AssumeDefaultVersionWhenUnspecified = true;
				o.DefaultApiVersion = ApiVersion.Default;
				o.ReportApiVersions = true;
				// o.ApiVersionReader = ApiVersionReader.Combine(
				// new QueryStringApiVersionReader("api-version"),
				// new HeaderApiVersionReader("X-Version"),
				// new MediaTypeApiVersionReader("ver"));
			});

			services.AddVersionedApiExplorer(
				options =>
				{
					options.GroupNameFormat = "'v'VVV";
					options.SubstituteApiVersionInUrl = true;
				});

			services.AddApplicationServices(_config);
			services.AddIdentityServices(_config);
			// Repositories

		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			// if (env.IsDevelopment())
			// {
			app.UseDeveloperExceptionPage();
			app.UseSwagger();
			app
			  .UseSwaggerUI(c =>
			  {
				  c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPIv5 v1");
				  c.RoutePrefix = string.Empty;
			  });

			app.UseMiddleware<ExceptionMiddleware>();
			// }

			// app.UseHttpsRedirection();
			
			app.UseCors(builder => builder.WithOrigins(
				"https://localhost:3000", "http://localhost:3000",
				"https://evsmart.netlify.app", "http://evsmart.netlify.app"
			).AllowAnyMethod().AllowAnyHeader().AllowCredentials());

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app
			  .UseEndpoints(endpoints =>
			  {
				  endpoints.MapControllers();
				  endpoints.MapHub<ChatHub>("chat");
			  });
		}
	}
}
