using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.TicketUsers;
using Application.TicketUsers.DTOs;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Controllers
{
	/// <summary>
	///	Ticker - User
	/// </summary>
	[Route("api/v{version:apiVersion}/tickets/{ticketid}/users")]
	public class TicketUserController : BaseApiController
	{
		/// <summary>
		/// [Authorize] [>= Moderator] Get Ticket Users
		/// </summary>
		[HttpGet]
		public async Task<ActionResult<List<TicketUserDTO>>> GetTicketUsers(Guid ticketid, [FromQuery] TicketUserQueryParams queryParams)
		{
			return HandleResult(await Mediator.Send(new List.Query() { ticketId = ticketid, queryParams = queryParams }));
		}

		/// <summary>
		/// [Authorize] [>= Student] Get Self Ticket User
		/// </summary>
		[HttpGet("me")]
		public async Task<ActionResult<TicketUserDTO>> GetSelfTicketUser(Guid ticketid)
		{
			return HandleResult(await Mediator.Send(new Details.Query { ticketId = ticketid, userId = Guid.Parse(User.GetUserId()) }));
		}

		/// <summary>
		/// [Authorize] [>= Moderator] Get Ticket User
		/// </summary>
		[HttpGet("{userid}")]
		public async Task<ActionResult<TicketUserDTO>> GetTicketUser(Guid ticketid, Guid userid)
		{
			return HandleResult(await Mediator.Send(new Details.Query { ticketId = ticketid, userId = userid }));
		}

		/// <summary>
		/// [Authorize] [> Student] Create Ticket User (Buy a ticket, 1 ticket/event)
		/// </summary>
		[Authorize]
		[HttpPost]
		public async Task<ActionResult> CreateEventUser(Guid ticketid, CreateTicketUserDTO dto)
		{
			return HandleResult(await Mediator.Send(new Create.Command { ticketId = ticketid, dto = dto }));
		}

		// /// <summary>
		// /// [Authorize] [> Moderator] Edit Event User (Edit user with role below them)
		// /// </summary>
		// [Authorize]
		// [HttpPut]
		// public async Task<ActionResult> EditEventUser(Guid eventid, EditEventUserDTO dto)
		// {
		// 	return HandleResult(await Mediator.Send(new Edit.Command { eventId = eventid, dto = dto }));
		// }


		// /// <summary>
		// /// [Authorize] [Creator] Delete Event User
		// /// </summary>
		// [Authorize]
		// [HttpDelete]
		// public async Task<ActionResult> DeleteEventUser([FromBody] Guid id)
		// {
		// 	return HandleResult(await Mediator.Send(new Delete.Command { Id = id }));
		// }
	}
}