using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Events;
using Application.Events.DTOs;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Controllers
{
	public class EventsController : BaseApiController
	{
		/// <summary>
		/// Get Events
		/// </summary>
		[HttpGet]
		public async Task<ActionResult<List<EventDTO>>> GetEvents([FromQuery] EventQueryParams queryParams)
		{
			return HandleResult(await Mediator.Send(new List.Query() { queryParams = queryParams }));
		}

		/// <summary>
		/// Get Event
		/// </summary>
		[HttpGet("{id}")]
		public async Task<ActionResult<DetailEventDTO>> GetEvent(Guid id)
		{
			return HandleResult(await Mediator.Send(new Details.Query { Id = id }));
		}

		/// <summary>
		/// [Admin Only] Create Event
		/// </summary>
		[Authorize(Roles = "Admin")]
		[HttpPost]
		public async Task<ActionResult> CreateEvent(CreateEventDTO Event)
		{
			return HandleResult(await Mediator.Send(new Create.Command { Event = Event }));
		}


		/// <summary>
		/// [Authorize] [> Moderator] Edit Event
		/// </summary>
		[Authorize]
		[HttpPut]
		public async Task<ActionResult> EditEvent(EditEventDTO dto)
		{
			return HandleResult(await Mediator.Send(new Edit.Command { dto = dto }));
		}


		/// <summary>
		/// [Authorize] [> Moderator] Delete Event
		/// </summary>
		[Authorize]
		[HttpDelete]
		public async Task<ActionResult> DeleteEvent([FromBody] Guid id)
		{
			return HandleResult(await Mediator.Send(new Delete.Command { Id = id }));
		}
	}
}