using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.DTOs;
using Application.Services;
using AutoMapper;
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
		private readonly RoleManager<IdentityRole<Guid>> _roleManager;

		public AuthController(TokenService tokenService,
						UserService userService,
						IConfiguration config,
						FirebaseService firebaseService,
						UserManager<User> userManager,
						RoleManager<IdentityRole<Guid>> roleManager,
						IMapper mapper
						)
		{
			_firebaseService = firebaseService;
			_tokenService = tokenService;
			this._userService = userService;
			_config = config;
			_userManager = userManager;
			this._roleManager = roleManager;
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

				var role = await _roleManager.FindByNameAsync("User");
				await _userManager.AddToRoleAsync(user, role.Name);
			}
			else
			{
				var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
				return Ok(await CreateUserObject(user, role));
			}
			return null;
		}

		private async Task<LoginResultDTO> CreateUserObject(User user, string role)
		{
			return new LoginResultDTO
			{
				Id = user.Id,
				DisplayName = user.DisplayName,
				Token = await _tokenService.CreateToken(user, role),
				Email = user.Email,
				Role = role,
				Image = user.ImageURL
			};
		}
	}
}