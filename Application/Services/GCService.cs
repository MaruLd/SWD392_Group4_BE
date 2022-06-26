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
			googleCred = GoogleCredential.FromJson("{\"type\": \"service_account\",\"project_id\": \"instant-land-354403\",\"private_key_id\": \"5a6681faecb86f1b04eaea54d77b034e0518b02f\",\"private_key\": \"-----BEGIN PRIVATE KEY-----\nMIIEvAIBADANBgkqhkiG9w0BAQEFAASCBKYwggSiAgEAAoIBAQCnVqIXvnFxej/o\nLpulcHT1cHUbaIfW+EUtGE64Yrtf6JcjB7k0+haQ1Qot/6z18OzjMGCMCB4AN4uf\nU2IzfY8Rq2kgJBnxmtVeFzK7tpaLOmnKXwdQEFm0Yuxoy7VnTLM0hhmudpED8jUv\nXB8GLuWPfcwEjy1z9TQWz2PnaeIpfQg/ZRr0Fkh7UXttk8gW2c+zqYi+nu7vHNkk\naEBxe+zJ8S8T/+YOO740yuFDFH1ETkIbmc2n3L+p9akd7ExwHv1Xw5LMFk8B6DlU\nwL+M5/YQ2dr2y2sC527Amh1UAwRx1CugGKQQWP02moMjPPeeuBX2t6luluFgKcOl\nLy63LcrDAgMBAAECggEACxfOogVRbuTcRK+GSObWEgk08i6xRpppt44TddyxsC9y\niXNzHyRvB4Xf6WG4HQuuIHduBwpotrn36wVzjpdFuzWiP+u0vpP1jgm6pr/5Zdxm\n8+/tw6x5zd/67Q2IBoOKo7I6FJsujNTrGqsgMWA9fpAnzKuTJ0s7e0GfTVFzGS3h\ntMI2splWxydWEMRVMWNHbyPkqDskI9jhA9jQp1o9Zs3uFzJ86gm+nYG9ljG6ucrk\nRMXvbhBS44wrFAumTKf/nk0uP5hK1zKEPeaK4dVv4Y3DtNGBysM834dPObcD6IU4\nTe4VAuN3VmmXa7kzDy+CzivBfv3jWpf0XSmo0qtTGQKBgQDc2z3YaHKjJBrMTBUa\nITcSZo8bkqqIomV4pee7+hIXLxceGb7gY+oTMXVf2Z5MMtoaT6z4jdPnAWF7wlPY\nG4yaqHaLk2QH3lNDGGrYdbX5ofFANQXOwqCCNJ5LZcKYe08JJ367/ebjTZhoaomb\n3ebMYq7IyT641PRwQ6K0KcTC+QKBgQDB90tEB5YDD7sTak2CSHTgdGqbID2QDHyj\nAdsk6Nm32Fmdp1d+19vWmk79IQYuCEEm9kAzDstcXRepPDRZj3MrIVUvt1p7uvB5\n4RLFN0pl2cpFsZWb/97EFDy2BzjBFAEnHgpAKrHYRlmHkSehYUQxHRc+0rGstrxa\n4xgaP9sumwKBgCqnSPVvjpxFT8ue8gBe49Tw67iGhdrnijNXzz51mGLT3GIa1Mtf\nYIieZ63ASJsswwSL2LsUIRWfJaWSLUEyaOBBostoBsyiOnBd2dy+NwvkI8SjuOBq\npDchVGT5XTC3t9brwsUpzxqdFM8trC/nFjjo4hqzCyMFNcQsu9KRsod5AoGAdaPK\nnmQLc6fiyo+6mr3fRRLWZWdUbz1jimnhw60F/x7Um22W/3Nj/fBw381btieDfGH1\n5gGfDVHYu+eryHXcX9hDlkEaO9jRaNo9TCeQk91XdJWUK50wOrQbbjtLDK9ZU6RA\ni1JZstZYDoTXCGW5uh/urUeUQzBBZM4/HAyGnukCgYBDqE2PM1zXTSJRG5daOAAD\n+jELXgewiZ4uxbHhavHHzDEKL0Ytt1B7fwpTfjX4KkZGZ9B2lc00ITMxtqHihwLx\nGYa+v+erdFnd2bu7nROxWcpBM2HLhsB8jLd7R1w9gAwUtINJhE9dRmR6FJxyCw6g\nB7kB3q/ZXS+lUkpAw05w9Q==\n-----END PRIVATE KEY-----\n\",\"client_email\": \"evsmart-csharp@instant-land-354403.iam.gserviceaccount.com\",\"client_id\": \"103381377583400944779\",\"auth_uri\": \"https://accounts.google.com/o/oauth2/auth\",\"token_uri\": \"https://oauth2.googleapis.com/token\",\"auth_provider_x509_cert_url\": \"https://www.googleapis.com/oauth2/v1/certs\",\"client_x509_cert_url\": \"https://www.googleapis.com/robot/v1/metadata/x509/evsmart-csharp%40instant-land-354403.iam.gserviceaccount.com\"}");
			client = StorageClient.Create(googleCred);
			var scopes = new string[] { "https://www.googleapis.com/auth/devstorage.read_write" };
			serviceCred = googleCred.CreateScoped(scopes).UnderlyingCredential as ServiceAccountCredential;
		}

		public async Task<String> UploadImage(IFormFile file, Guid key, String extension)
		{
			var bucket = await client.GetBucketAsync("evsmart_bucket_1");
			var result = await client.UploadObjectAsync(bucket.Name, $"images/{key.ToString().ToLower()}.{extension}", file.ContentType, file.OpenReadStream());
			if (result == null) return null;

			return key.ToString();
		}

		public async Task<String> GetImage(string key)
		{
			UrlSigner urlSigner = UrlSigner.FromServiceAccountCredential(serviceCred);

			RequestTemplate template = RequestTemplate
				.FromBucket("evsmart_bucket_1")
				.WithObjectName($"images/{key.ToLower()}")
				.WithHttpMethod(HttpMethod.Get);

			Options options = Options.FromDuration(TimeSpan.FromDays(1));
			string imageUrl = urlSigner.Sign(template, options);

			return imageUrl;
		}
	}
}