using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
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
		private readonly IConfiguration _config;
		private readonly UserManager<User> _userManager;

		public AuthController(TokenService tokenService, IConfiguration config, FirebaseService firebaseService, UserManager<User> userManager)
		{
			_firebaseService = firebaseService;
			_tokenService = tokenService;
			_config = config;
			_userManager = userManager;
		}

		[HttpPost]
		public async Task<IActionResult> GetTestToken([FromQuery] string email)
		{
			//return Ok("token : " + _config.GetValue<string>("JWTSecretKey"));
			await _userManager.CreateAsync(new User() { Email = email, UserName = "johnnymc2001", DisplayName = "johnnymc2001" });
			return Ok(_tokenService.CreateToken(email));
		}

		[HttpPost("auth-google")]
		public async Task<IActionResult> TestGoogleAuth([FromQuery] string token)
		{
			return Ok(await _firebaseService.VerifyIdToken(token));
		}
	}
}