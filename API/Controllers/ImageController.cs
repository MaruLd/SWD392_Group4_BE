using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Application.Core;
using Application.Services;
using Application.UserImages.DTOs;
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
		public async Task<ActionResult<UserImageDTO>> UploadImage([Required] IFormFile file)
		{
			if (file.Length > 10 * 1024 * 1024) return BadRequest("File size limit is 10 MB!");
			if (!file.ContentType.Contains("image")) return BadRequest("File is not a valid image!");

			var image = new UserImage() { UserId = Guid.Parse(User.GetUserId()) };
			var result = await _imageService.Insert(image);

			if (!result) return StatusCode(StatusCodes.Status500InternalServerError, "Something wrong, please try again!");
			var uploadResult = await _awsService.UploadImage(file, image.Id);

			var dto = new UserImageDTO()
			{
				Id = image.Id.ToString(),
				Url = "https://evsmart.herokuapp.com/api/v1/image?key=" + image.Id.ToString(),
				CreatedDate = image.CreatedDate
			};

			return Ok(dto);
		}

		/// <summary>
		/// Get Image
		/// </summary>
		[HttpGet]
		public async Task<ActionResult> GetImage(string key)
		{
			var notfoundImg = "https://i.imgur.com/goEUDMG.png";
			try
			{
				var id = Guid.Parse(key);
				var image = await _imageService.GetByID(id);
				if (image == null) return Redirect(notfoundImg);

				var url = await _awsService.GetImage(id.ToString().ToLower());
				return Redirect(url);
			}
			catch
			{
				return Redirect(notfoundImg);
			}
		}

		/// <summary>
		/// Test Upload Parameter
		/// </summary>
		[HttpPost("validate-upload")]
		public async Task<ActionResult> TestUploadImage([Required] IFormFile file)
		{
			if (file.Length > 10 * 1024 * 1024) return BadRequest("File size limit is 10 MB!");
			if (!file.ContentType.Contains("image")) return BadRequest("File is not a valid image!");

			return Ok("Image Is Valid!");
		}
	}
}