using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Events;
using Application.Events.DTOs;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace API.Controllers
{
	public class EventsController : BaseApiController
	{


		[HttpGet]
		public async Task<ActionResult<List<Event>>> GetEvents()
		{
			return await Mediator.Send(new List.Query());
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<Event>> GetEvent(int id)
		{
			var activity = await Mediator.Send(new Details.Query { Id = id });

			if (activity == null) return NotFound();

			return activity;
		}

		[HttpPost]
		public async Task<ActionResult> CreateEvent(Event Event)
		{
			return Ok(await Mediator.Send(new Create.Command { Event = Event }));
		}

		[HttpPut("{id}")]
		public async Task<ActionResult> EditEvent(int id, Event Event)
		{
			Event.Id = id;
			return Ok(await Mediator.Send(new Edit.Command { Event = Event }));
		}

		[HttpDelete("{id}")]
		public async Task<ActionResult> DeleteEvent(int id)
		{
			return Ok(await Mediator.Send(new Delete.Command { Id = id }));
		}
	}
}