using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.EventOrganizers.DTOs;
using Application.EventOrganizers;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Controllers
{
	[Route("api/v{version:apiVersion}/events/{eventid}/organizers")]
	public class EventOrganizerController : BaseApiController
	{
		/// <summary>
		/// Get Event Organizers
		/// </summary>
		[HttpGet]
		public async Task<ActionResult<List<EventOrganizerDTO>>> GetEventOrganizers(Guid eventid)
		{
			return HandleResult(await Mediator.Send(new List.Query() { eventId = eventid }));
		}

		/// <summary>
		/// Get Event Organizer
		/// </summary>
		[HttpGet("{id}")]
		public async Task<ActionResult<EventOrganizerDTO>> GetEventOrganizer(Guid eventid, Guid organizerid)
		{
			return HandleResult(await Mediator.Send(new Details.Query { eventId = eventid, organizerId = organizerid }));
		}

		/// <summary>
		/// [Authorize] [>= Moderator] Create Event Organizer
		/// </summary>
		[Authorize]
		[HttpPost]
		public async Task<ActionResult<EventOrganizerDTO>> CreateEventOrganizer(Guid eventid, CreateEventOrganizerDTO dto)
		{
			return HandleResult(await Mediator.Send(new Create.Command { eventId = eventid, dto = dto }));
		}

		// /// <summary>
		// /// [Authorize] [>= Moderator] Edit Event User (Edit user with role below them)
		// /// </summary>
		// [Authorize]
		// [HttpPut]
		// public async Task<ActionResult> EditEventUser(Guid eventid, EditEventUserDTO dto)
		// {
		// 	return HandleResult(await Mediator.Send(new Edit.Command { eventId = eventid, dto = dto }));
		// }


		/// <summary>
		/// [Authorize] [>= Moderator] Delete Event Organizer
		/// </summary>
		[Authorize]
		[HttpDelete]
		public async Task<ActionResult> DeleteEventOrganizer(Guid eventid, Guid organizerid)
		{
			return HandleResult(await Mediator.Send(new Delete.Command { eventId = eventid, organizerId = organizerid }));
		}
	}
}