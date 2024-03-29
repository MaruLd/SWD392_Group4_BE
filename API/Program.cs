using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Persistence;

namespace API
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var host = CreateHostBuilder(args).Build();
			using var scope = host.Services.CreateScope();
			var services = scope.ServiceProvider;

			try
			{
				var context = services.GetRequiredService<DataContext>();
				var userManager = services.GetRequiredService<UserManager<User>>();
				var config = services.GetRequiredService<IConfiguration>();
				var rm = services.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

				FirebaseApp.Create(new AppOptions()
				{
					Credential = GoogleCredential.FromJson("{\"type\": \"service_account\",\"project_id\": \"evsmart-se1503\",\"private_key_id\": \"3326d3a31220013f8a7f881528df970a12afde63\",\"private_key\": \"-----BEGIN PRIVATE KEY-----\nMIIEvgIBADANBgkqhkiG9w0BAQEFAASCBKgwggSkAgEAAoIBAQCVTlQGkSp00nEV\nloK4PrhMbSDiEE+3EH/nheuDXKBboINJICcVVzISRNQJl9vT4QoqXDTHzRgdI3Mp\nknGNuUorqMR6f9S7EhKGH0GoQkwK0Fcv5m/GQrxSD3FfuHSIWlz8F9ZGvbWEZoqx\nPqWUhwTi1PChHoaKWoORJ/tD9z+Vctbga79/FvEc91cjeL/eebq8KF57y3cAWs8e\n1dQ4QRZlbVGVk0JiMdkNKXkTGrrMxW+89lU3nqSMQ8Mj7AcD6ZYQRkHPgKZ7MI6j\n95ceuQGe7tGF+fQ7LgunG9cU7jZRmG6G/NZvTWpRmiDJhINAl9nJcw/NnV8ngMoI\nQTo0Lw3ZAgMBAAECggEAA/fqlqht77V2i9ZDNV5LZpKhpLM+HyruhLWZR9Wi2H9I\nxgfh/2NLgX34Ac+aEu9gu7VVh3hS0KweypHUd/8fCCU1Swk8mwKftetnmBniV149\nbFiXC3a+ISiSa5165siPafmV4y2ggRClqpDe9KFDFrwGBPj1/ABBuM5j02UODleO\nQaFvft3rZ+8rYo4iSzZ3cSZkmmwSrQ6FcIvUvyrOs4qWtB0ApQEIWmgBtp7E03dP\nBzwjhZnEqsWKgtfI57AzYcA1Tm01TAWVHr5PznzSydULez6AmCj2rxdINitEYRPx\nA7bEypKM5HPFuG53lpxzrVk69m4s55fWVWZn6W5fwQKBgQDFtl0GMVppFpugvWHY\nx9VI8gbcdRwvklQpvKRVtdn4JWvr/SQQtnRPupAfDgIctCgzrsWaDpOjex9vwSPg\nFp7iHe0ODzkr70ntU+d8vgMNr5RfcvaGcKz5BRY0xvdiPmAy9OQe3s6R7g/ffeu6\nqeWnoIAm+rEnoFMjjB0kQzQZOQKBgQDBUqo+JuOYl5KX9NN0thjtioT9fcLliEd1\nOwMSUbmfD+rsnSsM4TjAHNfE/c/RGFZTNUuNFqd+i1lc5PYRCH08dZlbJGvRz0Pe\n2vEyRtmyu7tlZ7RAPrIVens1zYt2ttFLgXjw+jkXXN2erG5efkUCBYtEFvvYCPxn\nSsMtkLm5oQKBgQCVXXMjpY9fCvRh2BEeu8F3DVl/nX3AtrScn2YrVmooXOUOcLyZ\nX625OgF9ZJDV7Ijemq+v9kk1XWPfgrM+rPg1bVRUpc2UUn+wKw4cIFSgN0BJZ8m0\nwVT8AArJrnLgJq14cagRZlP6zTXyque2qnnUTJ3kVMoXXLjShxzQ83BdiQKBgEdD\nD5pFf5QaG2GEUAYvsdSuQQOoPhWaPK0MCb/Q/FmT5oc+EEZ5JxA6EZd98AMls3yM\nosLpXOiauWAyzjnNJU0KFHOyY0Q94MjfcBWWZF0sMpHYvmsIMWEVeyGEGSqzjUcF\n/Oznb/AavV7mNPEEHq/2FzYMOt2RwmjY+EVtmejBAoGBAJ84w9jJNaoBwjXwiOo/\nLfn09DBHjKeXQVwSZsciugKVnzC9vQhtDjc4RqebkRmQtZdaZAR8o+52ojKQRi0N\nZBgp3gjnm/kVrb94qAfCiuF26tNXv9WZtpF2x9FlW4+lMBOWyofjh7evNaDyJZsx\n1bwSaxrXJj2mVsGgvotP8w/6\n-----END PRIVATE KEY-----\n\",\"client_email\": \"firebase-adminsdk-5sdgk@evsmart-se1503.iam.gserviceaccount.com\",\"client_id\": \"110369373675305924221\",\"auth_uri\": \"https://accounts.google.com/o/oauth2/auth\",\"token_uri\": \"https://oauth2.googleapis.com/token\",\"auth_provider_x509_cert_url\": \"https://www.googleapis.com/oauth2/v1/certs\",\"client_x509_cert_url\": \"https://www.googleapis.com/robot/v1/metadata/x509/firebase-adminsdk-5sdgk%40evsmart-se1503.iam.gserviceaccount.com\"}")
				});


				if (await rm.Roles.CountAsync() == 0)
				{
					await rm.CreateAsync(new IdentityRole<Guid>("User"));
					await rm.CreateAsync(new IdentityRole<Guid>("Admin"));
				}


				// await context.Database.MigrateAsync();
				// await Seed.SeedData(context, userManager);
				// await Seed.ClearSeedData(context, userManager);
			}
			catch (Exception ex)
			{
				var logger = services.GetRequiredService<ILogger<Program>>();
				logger.LogError(ex, "An error occured during migration;");
			}


			host.Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseStartup<Startup>();

				});
	}
}
