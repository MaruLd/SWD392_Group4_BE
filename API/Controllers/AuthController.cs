using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using API.DTOs;
using Application.Core;
using Application.Services;
using AutoMapper;
using Domain;
using FirebaseAdmin.Auth;
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
		private readonly UserFCMTokenService _userFCMTokenService;
		private readonly RedisConnection _redisConnection;

		public AuthController(TokenService tokenService,
						UserService userService,
						IConfiguration config,
						FirebaseService firebaseService,
						UserManager<User> userManager,
						RoleManager<IdentityRole<Guid>> roleManager,
						UserFCMTokenService userFCMTokenService,
						IMapper mapper,
						RedisConnection redisConnection
						)
		{
			_firebaseService = firebaseService;
			_tokenService = tokenService;
			this._userService = userService;
			_config = config;
			_userManager = userManager;
			this._roleManager = roleManager;
			this._userFCMTokenService = userFCMTokenService;
			this._redisConnection = redisConnection;
		}

		/// <summary>
		/// Login with google token and return a JWT token
		/// </summary>
		[HttpPost("auth-google")]
		public async Task<ActionResult<LoginResultDTO>> TestGoogleAuth([FromQuery] string token)
		{
			FirebaseToken claims = null;
			try
			{
				claims = await _firebaseService.VerifyIdToken(token);

				if (claims == null)
				{
					return BadRequest("Token not valid!");
				}
			}
			catch (Exception e)
			{
				return BadRequest("Token not valid!");
			}



			var email = claims.Claims.FirstOrDefault(c => c.Key == "email").Value.ToString();
			// if (email.Split("@").Last() != "fpt.edu.vn") return StatusCode(418, "The system currently only accepted [fpt.edu.vn] email!");
			var name = claims.Claims.FirstOrDefault(c => c.Key == "name").Value.ToString();
			var imgUrl = claims.Claims.FirstOrDefault(c => c.Key == "picture").Value.ToString();

			var user = await _userService.GetByEmail(email);

			if (user == null)
			{
				user = new User()
				{
					Email = email,
					UserName = email,
					DisplayName = name,
					ImageURL = imgUrl,
					Bean = 100
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

		/// <summary>
		/// Add Android's FCM Token to User
		/// </summary>

		[Authorize]
		[HttpPost("fcm-token")]
		public async Task<ActionResult> AddFCMToken([FromBody] string fcmToken)
		{
			var tokenInDb = await _userFCMTokenService.GetByFCMToken(fcmToken);
			if (tokenInDb != null) return BadRequest("Token already added!");

			if (!await _firebaseService.VerifyFCMToken(fcmToken)) return BadRequest("Token not valid!");

			var user = await _userService.GetByID(Guid.Parse(User.GetUserId()));
			if (user == null) return NotFound("User Not Found");

			var result = await _userFCMTokenService.Insert(new UserFCMToken()
			{
				UserId = user.Id,
				Token = fcmToken
			});

			if (!result) return StatusCode(StatusCodes.Status500InternalServerError, "Something wrong please try again!");
			return Ok("Token Added");
		}
		[HttpGet("weeee")]
		public async Task<ActionResult> TestWEEE()
		{
			Dictionary<String, Object> data = new Dictionary<string, Object>();
			data.Add("message", "Hello From Mars!");
			
			_redisConnection.AddToQueue("SendNotification_All", data);
			return Ok();
		}
	}


}