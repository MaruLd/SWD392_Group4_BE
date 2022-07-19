using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Application.Core;
using Application.Services;
using Application.UserImages.DTOs;
using AutoMapper;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
	public class ImageController : BaseApiController
	{
		private readonly GCService _gCService;
		private readonly UserImageService _imageService;

		public ImageController(GCService gCService, UserImageService imageService)
		{
			this._gCService = gCService;
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

			var image = new UserImage()
			{
				UserId = Guid.Parse(User.GetUserId())
			};

			var result = await _imageService.Insert(image);
			if (!result) return StatusCode(StatusCodes.Status500InternalServerError, "Something wrong, please try again!");

			var ext = file.FileName.Split(".").Last();

			if (
				ext.Contains("[") ||
				ext.ToLower().Contains("object") ||
				ext.ToLower().Contains("file") ||
				ext.Contains("]"))
			{
				ext = file.ContentType.Split("/").Last();
			}

			var uploadResult = await _gCService.UploadImage(file, image.Id, ext);

			if (uploadResult == null)
			{
				await _imageService.Delete(image);
				return StatusCode(StatusCodes.Status500InternalServerError, "Something wrong, please try again!");
			}


			image.ImageName = $"{image.Id}.{ext}";
			await _imageService.Update(image);

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
				if (image == null || image.ImageName == "") return Redirect(notfoundImg);

				var url = await _gCService.GetImage(image.ImageName);
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

		/// <summary>
		/// Get Image
		/// </summary>
		[HttpGet("user-image")]
		[Authorize]
		public async Task<ActionResult<List<UserImageDTO>>> GetImages()
		{
			var list = new List<UserImageDTO>();
			var images = await _imageService.GetAllFromUser(Guid.Parse(User.GetUserId()));

			images.ForEach(image =>
			{
				var dto = new UserImageDTO()
				{
					Id = image.Id.ToString(),
					Url = "https://evsmart.herokuapp.com/api/v1/image?key=" + image.Id.ToString(),
					CreatedDate = image.CreatedDate
				};
				list.Add(dto);
			});

			return list;
		}
	}
}