using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon;
using Amazon.S3;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
	public class ImageController : BaseApiController
	{
		private readonly IConfiguration _config;

		public ImageController(IConfiguration config)
		{
			this._config = config;
		}

		[HttpPost]
		public async Task<ActionResult> UploadImage(IFormFile files)
		{
			var config = new AmazonS3Config
			{
				AuthenticationRegion = _config.GetValue<string>("AWS:AccessKey"), // Should match the `MINIO_REGION` environment variable.
				ServiceURL = "http://localhost:9000", // replace http://localhost:9000 with URL of your MinIO server
				ForcePathStyle = true // MUST be true to work correctly with MinIO server
			};
			var accessKey = _config.GetValue<string>("AWS:AccessKey");
			var secretKey = _config.GetValue<string>("AWS:SecretKey");

			var amazonS3Client = new AmazonS3Client(accessKey, secretKey, config);
			var s3Client = new AmazonS3Client();

			return Ok();



		}
	}
}