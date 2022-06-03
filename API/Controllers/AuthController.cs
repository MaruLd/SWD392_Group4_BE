using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.DTOs;
using Application.Services;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
	public class AuthController : BaseApiController
	{
		private FirebaseService _firebaseService;
		private TokenService _tokenService;
		private readonly UserService _userService;
		private readonly IConfiguration _config;
		private readonly UserManager<User> _userManager;

		public AuthController(TokenService tokenService,
						UserService userService,
						IConfiguration config,
						FirebaseService firebaseService,
						UserManager<User> userManager)
		{
			_firebaseService = firebaseService;
			_tokenService = tokenService;
			this._userService = userService;
			_config = config;
			_userManager = userManager;
		}

		[HttpPost]
		public async Task<IActionResult> GetTestToken([FromQuery] string email)
		{
			//return Ok("token : " + _config.GetValue<string>("JWTSecretKey"));
			var user = await _userService.GetByEmail(email);
			if (user == null)
			{
				await _userManager.CreateAsync(new User() { Email = email, UserName = "AUserName", DisplayName = "ADisplayName" });
			}

			return Ok(_tokenService.CreateTestToken(email));
		}

		[HttpPost("auth-google")]
		public async Task<IActionResult> TestGoogleAuth([FromQuery] string token)
		{
			var claims = await _firebaseService.VerifyIdToken(token);

			if (claims == null)
			{
				return BadRequest(Results.BadRequest("Token not valid!"));
			}

			var email = claims.Claims.FirstOrDefault(c => c.Key == "email").Value.ToString();
			var name = claims.Claims.FirstOrDefault(c => c.Key == "name").Value.ToString();

			var user = await _userService.GetByEmail(email);

			if (user == null)
			{
				user = new User()
				{
					Email = email,
					UserName = email,
					DisplayName = name
				};

				await _userManager.CreateAsync(user);
			}

			return Ok(CreateUserObject(user));
		}

		private LoginResultDTO CreateUserObject(User user)
		{
			return new LoginResultDTO
			{
				Id = user.Id,
				DisplayName = user.DisplayName,
				Token = _tokenService.CreateToken(user),
				Email = user.Email,
				Image = user.ImageURL
			};
		}
	}
}