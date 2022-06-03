using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Organizers;
using Application.Events.DTOs;
using Application.Organizers.DTOs;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Controllers
{
	public class OrganizerController : BaseApiController
	{
		[HttpGet]
		public async Task<ActionResult<List<OrganizerDTO>>> GetEvents([FromQuery] OrganizerQueryParams queryParams)
		{
			return HandleResult(await Mediator.Send(new List.Query() { queryParams = queryParams }));
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<OrganizerDTO>> GetEvent(Guid id)
		{
			return HandleResult(await Mediator.Send(new Details.Query { Id = id }));
		}
		
		// [HttpPost]
		// public async Task<ActionResult> CreateEvent(CreateEventDTO Event)
		// {
		// 	return HandleResult(await Mediator.Send(new Create.Command { Event = Event }));
		// }

		// [HttpPut("{id}")]
		// public async Task<ActionResult> EditEvent(Guid id, EditEventDTO Event)
		// {
		// 	return HandleResult(await Mediator.Send(new Edit.Command { eventId = id, Event = Event }));
		// }

		// [HttpDelete("{id}")]
		// public async Task<ActionResult> DeleteEvent(Guid id)
		// {
		// 	return HandleResult(await Mediator.Send(new Delete.Command { Id = id }));
		// }
	}
}