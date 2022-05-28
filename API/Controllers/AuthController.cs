using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
	public class AuthController : BaseApiController
	{
		private TokenService _tokenService;
		private readonly IConfiguration _config;

		public AuthController(TokenService tokenService, IConfiguration config)
		{
			_tokenService = tokenService;
			_config = config;
		}

		[HttpPost]
		public async Task<IActionResult> GetTestToken([FromQuery] string email)
		{
            //return Ok("token : " + _config.GetValue<string>("JWTSecretKey"));
            return Ok(_tokenService.CreateToken(email));
        }
	}
}