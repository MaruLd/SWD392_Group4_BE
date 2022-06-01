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
using Persistence.Params;

namespace API.Controllers
{
	[Authorize]
	public class TicketsController : BaseApiController
	{
		[HttpGet]
		public async Task<ActionResult<List<Ticket>>> GetTickets([FromQuery] ListTicketDTO dto)
		{
			return HandleResult(await Mediator.Send(new List.Query() { dto = dto }));
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<Ticket>> GetTicket(Guid id)
		{

			return HandleResult(await Mediator.Send(new Details.Query { Id = id }));
		}

		[HttpPost]
		public async Task<ActionResult> CreateTicket(CreateTicketDTO Ticket)
		{
			return HandleResult(await Mediator.Send(new Create.Command { dto = Ticket }));
		}

		[HttpPut("{id}")]
		public async Task<ActionResult> EditTicket(Guid id, EditTicketDTO Ticket)
		{
			Ticket.Id = id;
			return HandleResult(await Mediator.Send(new Edit.Command { dto = Ticket }));
		}

		[HttpDelete("{id}")]
		public async Task<ActionResult> DeleteTicket(Guid id)
		{
			return HandleResult(await Mediator.Send(new Delete.Command { Id = id }));
		}
	}
}