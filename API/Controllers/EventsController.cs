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
		[HttpGet]
		public async Task<ActionResult<List<EventDTO>>> GetEvents([FromQuery] EventQueryParams queryParams)
		{
			return HandleResult(await Mediator.Send(new List.Query() { queryParams = queryParams }));
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<EventDTO>> GetEvent(Guid id)
		{
			return HandleResult(await Mediator.Send(new Details.Query { Id = id }));
		}


		/// <summary>
		/// [Authorize]
		/// </summary>
		[Authorize(Roles = "Admin")]
		[HttpPost]
		public async Task<ActionResult> CreateEvent(CreateEventDTO Event)
		{
			return HandleResult(await Mediator.Send(new Create.Command { Event = Event }));
		}


		/// <summary>
		/// [Authorize]
		/// </summary>
		[Authorize(Roles = "Admin")]
		[HttpPut]
		public async Task<ActionResult> EditEvent(EditEventDTO dto)
		{
			return HandleResult(await Mediator.Send(new Edit.Command { dto = dto }));
		}


		/// <summary>
		/// [Authorize]
		/// </summary>
		[Authorize(Roles = "Admin")]
		[HttpDelete]
		public async Task<ActionResult> DeleteEvent([FromBody] Guid id)
		{
			return HandleResult(await Mediator.Send(new Delete.Command { Id = id }));
		}
	}
}