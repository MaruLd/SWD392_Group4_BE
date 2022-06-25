using System.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using API.DTOs;
using Microsoft.AspNetCore.Identity;
using Domain;
using Application.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Application.Users.DTOs;
using Application.Users;
using Application.Core;
using Application.TicketUsers.DTOs;
using Application.Tickets.DTOs;

namespace API.Controllers
{
	[ApiController]
	[Route("api/v{version:apiVersion}/[controller]")]
	[ApiVersion("1.0")]
	public class UserController : BaseApiController
	{
		private readonly UserService _userService;

		public UserController(UserService userService)
		{
			this._userService = userService;
		}

		/// <summary>
		/// [Admin Only] Get User List
		/// </summary>
		[Authorize(Roles = "Admin")]
		[HttpGet]
		public async Task<ActionResult<List<UserDTO>>> GetUsers([FromQuery] UserQueryParams queryParams)
		{
			return HandleResult(await Mediator.Send(new List.Query() { queryParams = queryParams }));
		}

		/// <summary>
		/// [Authorize] Get Current User Info
		/// </summary>
		[Authorize]
		[HttpGet("me")]
		public async Task<ActionResult<UserDTO>> GetYourself()
		{
			return HandleResult(await Mediator.Send(new Details.Query { Id = Guid.Parse(User.GetUserId()) }));
		}

		/// <summary>
		/// [Authorize] Get Current User Tickets
		/// </summary>
		[Authorize]
		[HttpGet("me/tickets")]
		public async Task<ActionResult<List<SelfTicketDTO>>> GetYourTickets([FromQuery] TickerUserSelfQueryParams queryParams)
		{
			return HandleResult(await Mediator.Send(new ListSelfTickets.Query { userId = Guid.Parse(User.GetUserId()), queryParams = queryParams }));
		}

		/// <summary>
		/// [Authorize] Get Current User Events
		/// </summary>
		[Authorize]
		[HttpGet("me/events")]
		public async Task<ActionResult<List<SelfEventDTO>>> GetYourEvents([FromQuery] EventSelfQueryParams queryParams)
		{
			return HandleResult(await Mediator.Send(new ListSelfEvents.Query { userId = Guid.Parse(User.GetUserId()), queryParams = queryParams }));
		}

		/// <summary>
		/// [Authorize] Edit User information
		/// </summary>
		[Authorize]
		[HttpPut]
		public async Task<ActionResult<UserDTO>> EditUser(EditUserDTO dto)
		{
			return HandleResult(await Mediator.Send(new Edit.Command { dto = dto }));
		}
	}
}