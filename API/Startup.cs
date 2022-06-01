

using API.Extensions;
using API.Middleware;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
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
      services.AddControllers().AddNewtonsoftJson(o =>
        {
          o.SerializerSettings.Converters.Add(new StringEnumConverter
          {
            CamelCaseText = true
          });
          o.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        });
      services.AddApiVersioning(o =>
{
  o.AssumeDefaultVersionWhenUnspecified = true;
  o.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
  o.ReportApiVersions = true;
  o.ApiVersionReader = ApiVersionReader.Combine(
      new QueryStringApiVersionReader("api-version"),
      new HeaderApiVersionReader("X-Version"),
      new MediaTypeApiVersionReader("ver"));
	o.ReportApiVersions = true;
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
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app
          .UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPIv5 v1"));

        app.UseMiddleware<ExceptionMiddleware>();
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
