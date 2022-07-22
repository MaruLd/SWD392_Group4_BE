using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using static Google.Apis.Auth.OAuth2.ServiceAccountCredential;
using static Google.Cloud.Storage.V1.UrlSigner;

namespace Application.Services
{
	public class GCService
	{
		StorageClient client;
		GoogleCredential googleCred;
		ServiceAccountCredential serviceCred;
		// AmazonS3Client client;
		public GCService(IConfiguration configuration)
		{
			googleCred = GoogleCredential.FromJson("InsertKeyHere");
			client = StorageClient.Create(googleCred);
			var scopes = new string[] { "https://www.googleapis.com/auth/devstorage.read_write" };
			serviceCred = googleCred.CreateScoped(scopes).UnderlyingCredential as ServiceAccountCredential;
		}

		public async Task<String> UploadImage(IFormFile file, Guid key, String extension)
		{


			var bucket = await client.GetBucketAsync("InsertKeyHere");
			var result = await client.UploadObjectAsync(bucket.Name, $"images/{key.ToString().ToLower()}.{extension}", file.ContentType, file.OpenReadStream());
			if (result == null) return null;

			return key.ToString();
		}

		public async Task<String> GetImage(string key)
		{
			UrlSigner urlSigner = UrlSigner.FromServiceAccountCredential(serviceCred);

			RequestTemplate template = RequestTemplate
				.FromBucket("InsertKeyHere")
				.WithObjectName($"images/{key.ToLower()}")
				.WithHttpMethod(HttpMethod.Get);

			Options options = Options.FromDuration(TimeSpan.FromDays(1));
			string imageUrl = urlSigner.Sign(template, options);

			return imageUrl;
		}
	}
}