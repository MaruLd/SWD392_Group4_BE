using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Tickets;
using Application.Tickets.DTOs;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace API.Controllers
{
	[Authorize]
	public class TicketsController : BaseApiController
	{
		/// <summary>
		/// Get Tickets
		/// </summary>
		[HttpGet]
		public async Task<ActionResult<List<TicketDTO>>> GetTickets([FromQuery] TicketQueryParams queryParams)
		{
			return HandleResult(await Mediator.Send(new List.Query() { queryParams = queryParams }));
		}

		/// <summary>
		/// Get Ticket
		/// </summary>
		[HttpGet("{id}")]
		public async Task<ActionResult<TicketDTO>> GetTicket(Guid id)
		{
			return HandleResult(await Mediator.Send(new Details.Query { Id = id }));
		}

		/// <summary>
		/// [Authorize] [>= Moderator] Create Ticket
		/// </summary>
		[Authorize]
		[HttpPost]
		public async Task<ActionResult> CreateTicket(CreateTicketDTO Ticket)
		{
			return HandleResult(await Mediator.Send(new Create.Command { dto = Ticket }));
		}


		/// <summary>
		/// [Authorize] [>= Moderator] Edit Ticket
		/// </summary>
		[Authorize]
		[HttpPut]
		public async Task<ActionResult> EditTicket(EditTicketDTO dto)
		{
			return HandleResult(await Mediator.Send(new Edit.Command { dto = dto }));
		}

		/// <summary>
		/// [Authorize] [>= Moderator] Delete Ticket
		/// </summary>
		[Authorize]
		[HttpDelete]
		public async Task<ActionResult> DeleteTicket([FromBody] Guid id)
		{
			return HandleResult(await Mediator.Send(new Delete.Command { ticketId = id }));
		}
	}
}