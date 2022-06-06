using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
	public class ImageController : BaseApiController
	{
		private readonly AWSService _awsService;

		public ImageController(IConfiguration config, AWSService awsService)
		{
			this._awsService = awsService;
		}

		/// <summary>
		/// [Authorize] Upload Image
		/// </summary>
		[HttpPost]
		[Authorize]
		public async Task<ActionResult> UploadImage(IFormFile files)
		{
			var key = await _awsService.UploadImage(files);
			//commensing the transfer
			return Ok(key);
		}

		/// <summary>
		/// Get Image
		/// </summary>
		[HttpGet]
		public async Task<ActionResult> GetImage(string key)
		{
			var url = await _awsService.GetImage(key);
			//commensing the transfer
			return Redirect(url);
		}
	}
}