using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.EventUsers;
using Application.EventUsers.DTOs;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Controllers
{
	[Route("api/v{version:apiVersion}/events/{eventid}/users")]
	public class EventUsersController : BaseApiController
	{
		/// <summary>
		/// [Authorize] [> Moderator] Get Event Users
		/// </summary>
		[HttpGet]
		[Authorize]
		public async Task<ActionResult<List<EventUserDTO>>> GetEventUsers(Guid eventid, [FromQuery] EventUserQueryParams queryParams)
		{
			return HandleResult(await Mediator.Send(new List.Query() { eventId = eventid, queryParams = queryParams }));
		}

		/// <summary>
		///[Authorize] [>= Student] Get Self Event Users
		/// </summary>
		[HttpGet("me")]
		[Authorize]
		public async Task<ActionResult<EventUserDTO>> GetEventUser(Guid eventid)
		{
			return HandleResult(await Mediator.Send(new Details.Query { eventId = eventid, userId = Guid.Parse(User.GetUserId()) }));
		}

		/// <summary>
		///[Authorize] [>= Moderator] Get Event Users
		/// </summary>
		[HttpGet("{userid}")]
		[Authorize]
		public async Task<ActionResult<EventUserDTO>> GetEventUser(Guid eventid, Guid userid)
		{
			return HandleResult(await Mediator.Send(new Details.Query { eventId = eventid, userId = userid }));
		}

		/// <summary>
		/// [Authorize] [Creator] Create Event User
		/// </summary>
		[Authorize]
		[HttpPost]
		public async Task<ActionResult> CreateEventUser(Guid eventid, CreateEventUserDTO dto)
		{
			return HandleResult(await Mediator.Send(new Create.Command { eventId = eventid, dto = dto }));
		}


		/// <summary>
		/// [Authorize] [>= Moderator] Edit Event User (Edit user with role below them)
		/// </summary>
		[Authorize]
		[HttpPut]
		public async Task<ActionResult> EditEventUser(Guid eventid, EditEventUserDTO dto)
		{
			return HandleResult(await Mediator.Send(new Edit.Command { eventId = eventid, dto = dto }));
		}


		/// <summary>
		/// [Authorize] [Creator] Delete Event User
		/// </summary>
		[Authorize]
		[HttpDelete]
		public async Task<ActionResult> DeleteEventUser([FromBody] Guid id)
		{
			return HandleResult(await Mediator.Send(new Delete.Command { Id = id }));
		}
	}
}