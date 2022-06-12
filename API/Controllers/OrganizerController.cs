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
		/// <summary>
		/// Get Organizers
		/// </summary>
		[HttpGet]
		public async Task<ActionResult<List<OrganizerDTO>>> GetOrganizers([FromQuery] OrganizerQueryParams queryParams)
		{
			return HandleResult(await Mediator.Send(new List.Query() { queryParams = queryParams }));
		}

		/// <summary>
		/// Get Organizer
		/// </summary>
		[HttpGet("{id}")]
		public async Task<ActionResult<OrganizerDTO>> GetOrganzier(Guid id)
		{
			return HandleResult(await Mediator.Send(new Details.Query { Id = id }));
		}

		/// <summary>
		/// [Admin Only] Create Organizer
		/// </summary>
		[Authorize(Roles = "Admin")]
		[HttpPost]
		public async Task<ActionResult> CreateOrganizer(CreateOrganizerDTO dto)
		{
			return HandleResult(await Mediator.Send(new Create.Command { dto = dto }));
		}

		/// <summary>
		/// [Admin Only] Edit Organizer
		/// </summary>
		[Authorize(Roles = "Admin")]
		[HttpPut]
		public async Task<ActionResult> EditOrganizer(EditOrganizerDTO dto)
		{
			return HandleResult(await Mediator.Send(new Edit.Command { dto = dto }));
		}

		/// <summary>
		/// [Admin Only] Delete Organizer
		/// </summary>
		[Authorize(Roles = "Admin")]
		[HttpDelete]
		public async Task<ActionResult> DeleteOrganizer([FromBody] Guid id)
		{
			return HandleResult(await Mediator.Send(new Delete.Command { Id = id }));
		}
	}
}