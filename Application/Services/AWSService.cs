using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Application.Services
{
	public class AWSService
	{
		AmazonS3Client client;
		public AWSService(IConfiguration configuration)
		{
			fetchClient();
		}

		private void fetchClient()
		{
			if (client == null)
			{
				var amazonConfig = new AmazonS3Config
				{
					RegionEndpoint = RegionEndpoint.APSoutheast1,
					ForcePathStyle = true
				};
				// var accessKey = configuration.GetValue<string>("AWS:AccessKey");
				var accessKey = "AKIAYFYSY4MYV6W7Z3X3";
				// var secretKey = configuration.GetValue<string>("AWS:SecretKey");
				var secretKey = "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";

				var credentials = new BasicAWSCredentials(accessKey, secretKey);
				client = new AmazonS3Client(credentials, amazonConfig);
			}
		}

		public async Task<String> UploadImage(IFormFile file, Guid key)
		{
			fetchClient();
			TransferUtility utility = new TransferUtility(client);
			TransferUtilityUploadRequest request = new TransferUtilityUploadRequest();

			var listBucketResponse = await client.ListBucketsAsync();
			var bucket = listBucketResponse.Buckets[0];

			request.BucketName = bucket.BucketName;
			// request.ContentType = file.ContentType;
			request.Key = key.ToString();
			request.InputStream = file.OpenReadStream();
			// request.CannedACL = S3CannedACL.PublicRead;

			await utility.UploadAsync(request);

			return key.ToString();
		}

		public async Task<String> GetImage(string key)
		{
			fetchClient();
			var listBucketResponse = await client.ListBucketsAsync();
			var bucket = listBucketResponse.Buckets[0];

			var signedRequest = new GetPreSignedUrlRequest()
			{
				BucketName = bucket.BucketName,
				Key = key,
				Expires = DateTime.Now.AddHours(1)
			};

			var imageUrl = client.GetPreSignedURL(signedRequest);
			return imageUrl;
		}
	}
}