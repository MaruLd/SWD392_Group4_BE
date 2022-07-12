using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Events;
using Application.Events.DTOs;
using Application.Events.StateMachine;
using Domain;
using Domain.Enums;
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
		/// Get Events (Use for public get, wont show any DRAFT event). For editing purpose please use /me/event)
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
		public async Task<ActionResult<EventDTO>> CreateEvent(CreateEventDTO dto)
		{
			return HandleResult(await Mediator.Send(new Create.Command { dto = dto }));
		}


		/// <summary>
		/// [Authorize] [>= Moderator] Edit Event
		/// </summary>
		[Authorize]
		[HttpPut]
		public async Task<ActionResult> EditEvent(EditEventDTO dto)
		{
			return HandleResult(await Mediator.Send(new Edit.Command { dto = dto }));
		}


		/// <summary>
		/// [Authorize] [Creator] Delete Event
		/// </summary>
		[Authorize]
		[HttpDelete]
		public async Task<ActionResult> DeleteEvent([FromBody] Guid id)
		{
			return HandleResult(await Mediator.Send(new Delete.Command { Id = id }));
		}

		/// <summary>
		/// [Authorize] [>= Moderator] Patch Event State
		/// </summary>
		[Authorize]
		[HttpPatch]
		public async Task<ActionResult> PatchEventState([FromBody] PatchEventDTO dto)
		{
			return HandleResult(await Mediator.Send(new Patch.Command { dto = dto }));
		}

		/// <summary>
		/// [Authorize] [>= Moderator] Get Checkin/Checkout Code
		/// </summary>
		[Authorize]
		[HttpGet("get-code")]
		public async Task<ActionResult<EventCodeDTO>> GetEventCode([FromQuery] EventCodeParams dto)
		{
			return HandleResult(await Mediator.Send(new GetCode.Query { dto = dto }));
		}
	}
}