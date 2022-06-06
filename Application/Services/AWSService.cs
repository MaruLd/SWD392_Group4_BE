using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
			var amazonConfig = new AmazonS3Config
			{
				AuthenticationRegion = configuration.GetValue<string>("AWS:Region"),
				ForcePathStyle = true
			};
			var accessKey = configuration.GetValue<string>("AWS:AccessKey");
			var secretKey = configuration.GetValue<string>("AWS:SecretKey");

			client = new AmazonS3Client(accessKey, secretKey, amazonConfig);

		}

		public async Task<String> UploadImage(IFormFile files)
		{
			TransferUtility utility = new TransferUtility(client);
			TransferUtilityUploadRequest request = new TransferUtilityUploadRequest();

			var listBucketResponse = await client.ListBucketsAsync();
			var bucket = listBucketResponse.Buckets[0];

			var key = Guid.NewGuid().ToString();

			request.BucketName = bucket.BucketName;
			request.Key = key;
			request.InputStream = files.OpenReadStream();
			// request.CannedACL = S3CannedACL.PublicRead;

			await utility.UploadAsync(request);

			return key;
		}

		public async Task<String> GetImage(string key)
		{
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