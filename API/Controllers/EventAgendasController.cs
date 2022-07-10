using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.EventAgendas.DTOs;
using Application.EventAgendas;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Controllers
{
	[Route("api/v{version:apiVersion}/events/{eventId}/agenda")]
	public class EventAgendasController : BaseApiController
	{
		/// <summary>
		/// Get Event Agendas
		/// </summary>
		[HttpGet]
		public async Task<ActionResult<List<EventAgendaDTO>>> GetEventAgendas(
			Guid eventId,
			[FromQuery] EventAgendaQueryParams queryParams
			)
		{
			return HandleResult(await Mediator.Send(new List.Query() { eventid = eventId, queryParams = queryParams }));
		}

		/// <summary>
		/// Get Event Agenda
		/// </summary>
		[HttpGet("{id}")]
		public async Task<ActionResult<EventAgendaDTO>> GetEventAgenda(Guid id)
		{
			return HandleResult(await Mediator.Send(new Details.Query { id = id }));
		}

		/// <summary>
		/// [Authorize] [>= Moderator] Create Event Agenda
		/// </summary>
		[Authorize]
		[HttpPost]
		public async Task<ActionResult> CreateEventAgenda(
			 Guid eventId,
			[FromBody] CreateEventAgendaDTO dto
			)
		{
			return HandleResult(await Mediator.Send(new Create.Command { eventId = eventId, dto = dto }));
		}

		/// <summary>
		/// [Authorize] [>= Moderator] Edit Event Agenda
		/// </summary>
		[Authorize]
		[HttpPut]
		public async Task<ActionResult> EditEventAgenda(EditEventAgendaDTO dto)
		{
			return HandleResult(await Mediator.Send(new Edit.Command { dto = dto }));
		}

		/// <summary>
		/// [Authorize] [>= Moderator] Delete Event Agenda
		/// </summary>
		[Authorize]
		[HttpDelete]
		public async Task<ActionResult> DeleteEventAgenda([FromBody] Guid id)
		{
			return HandleResult(await Mediator.Send(new Delete.Command { Id = id }));
		}
	}
}