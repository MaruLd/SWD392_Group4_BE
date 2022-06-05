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
	[Route("api/v{version:apiVersion}/event/{eventId}/agenda")]
	public class EventAgendasController : BaseApiController
	{
		[HttpGet]
		public async Task<ActionResult<List<EventAgendaDTO>>> GetEventsAgenda(
			Guid eventId,
			[FromQuery] EventAgendaQueryParams queryParams
			)
		{
			queryParams.EventId = eventId;
			return HandleResult(await Mediator.Send(new List.Query() { queryParams = queryParams }));
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<EventAgendaDTO>> GetEventAgenda(Guid id)
		{
			return HandleResult(await Mediator.Send(new Details.Query { id = id }));
		}

		[Authorize(Roles = "Admin")]
		[HttpPost]
		public async Task<ActionResult> CreateEvent(
			 Guid eventId,
			[FromBody] CreateEventAgendaDTO dto
			)
		{
			return HandleResult(await Mediator.Send(new Create.Command { eventId = eventId, dto = dto }));
		}

		[Authorize(Roles = "Admin")]
		[HttpPut]
		public async Task<ActionResult> EditEvent(EditEventAgendaDTO dto)
		{
			return HandleResult(await Mediator.Send(new Edit.Command { dto = dto }));
		}

		[Authorize(Roles = "Admin")]
		[HttpDelete]
		public async Task<ActionResult> DeleteEvent([FromBody] Guid id)
		{
			return HandleResult(await Mediator.Send(new Delete.Command { Id = id }));
		}
	}
}