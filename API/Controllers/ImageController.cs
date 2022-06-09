using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Application.Core;
using Application.Services;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
	public class ImageController : BaseApiController
	{
		private readonly AWSService _awsService;
		private readonly UserImageService _imageService;

		public ImageController(AWSService awsService, UserImageService imageService)
		{
			this._awsService = awsService;
			this._imageService = imageService;
		}

		/// <summary>
		/// [Authorize] Upload Image
		/// </summary>
		[HttpPost]
		[Authorize]
		public async Task<ActionResult> UploadImage(IFormFile file)
		{
			var image = new UserImage() { UserId = Guid.Parse(User.GetUserId()) };
			var result = await _imageService.Insert(image);

			if (!result) return StatusCode(StatusCodes.Status500InternalServerError, "Something wrong, please try again!");
			var uploadResult = await _awsService.UploadImage(file, image.Id);
			return Ok(image.Id);
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