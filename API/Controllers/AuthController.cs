using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using API.DTOs;
using Application.Core;
using Application.Services;
using AutoMapper;
using Domain;
using Microsoft.AspNetCore.Authorization;
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

		/// <summary>
		/// Login with google token and return a JWT token
		/// </summary>
		[HttpPost("auth-google")]
		public async Task<ActionResult<LoginResultDTO>> TestGoogleAuth([FromQuery] string token)
		{
			var claims = await _firebaseService.VerifyIdToken(token);

			if (claims == null)
			{
				return BadRequest(Results.BadRequest("Token not valid!"));
			}

			var email = claims.Claims.FirstOrDefault(c => c.Key == "email").Value.ToString();

			// if (email.Split("@").Last() != "fpt.edu.vn") return StatusCode(418, "The system currently only accepted [fpt.edu.vn] email!");
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
				if (role == null) role = "User";
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

		[Authorize]
		[HttpPost("change-role")]
		public async Task<ActionResult> ChangeRole([FromQuery] string roleName)
		{
			var user = await _userService.GetByID(Guid.Parse(User.GetUserId()));
			var currentRole = await _userManager.GetRolesAsync(user);

			var role = await _roleManager.FindByNameAsync(roleName);
			if (role == null) return NotFound();
			
			await _userManager.RemoveFromRoleAsync(user, currentRole.First());
			await _userManager.AddToRoleAsync(user, role.Name);
			return Ok(user);
		}

		[HttpGet("test-message")]
		public async Task<ActionResult> SendTestMessage([FromQuery] string message)
		{
			return Ok(await _firebaseService.SendMessage(message));
		}
	}


}