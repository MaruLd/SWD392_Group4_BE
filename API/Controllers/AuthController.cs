using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
	public class AuthController : BaseApiController
	{
		private FirebaseService _firebaseService;
		private TokenService _tokenService;
		private readonly IConfiguration _config;

		public AuthController(TokenService tokenService, IConfiguration config, FirebaseService firebaseService)
		{
			_firebaseService = firebaseService;
			_tokenService = tokenService;
			_config = config;
		}

		[HttpPost]
		public async Task<IActionResult> GetTestToken([FromQuery] string email)
		{
            //return Ok("token : " + _config.GetValue<string>("JWTSecretKey"));
            return Ok(_tokenService.CreateToken(email));
        }

		[HttpPost("auth-google")]
		public async Task<IActionResult> TestGoogleAuth([FromQuery] string token) {
			return Ok(await _firebaseService.VerifyIdToken(token));
		}
	}
}