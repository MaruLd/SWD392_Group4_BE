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
using Persistence.Params;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Controllers
{
	public class EventsController : BaseApiController
	{
		[HttpGet]
		public async Task<ActionResult<List<EventDTO1>>> GetEvents([FromQuery] ListEventDTO dto)
		{
			return HandleResult(await Mediator.Send(new List.Query() { dto = dto }));
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetEvent(Guid id)
		{
			return HandleResult(await Mediator.Send(new Details.Query { Id = id }));
		}

		[HttpPost]
		public async Task<ActionResult> CreateEvent(Event Event)
		{
			return HandleResult(await Mediator.Send(new Create.Command { Event = Event }));
		}

		[HttpPut("{id}")]
		public async Task<ActionResult> EditEvent(Guid id, Event Event)
		{
			Event.Id = id;
			return HandleResult(await Mediator.Send(new Edit.Command { Event = Event }));
		}

		[HttpDelete("{id}")]
		public async Task<ActionResult> DeleteEvent(Guid id)
		{
			return HandleResult(await Mediator.Send(new Delete.Command { Id = id }));
		}
	}
}