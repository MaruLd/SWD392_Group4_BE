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

		[Authorize(Roles = "Admin")]
		[HttpPost]
		public async Task<ActionResult> CreateEvent(CreateOrganizerDTO dto)
		{
			return HandleResult(await Mediator.Send(new Create.Command { dto = dto }));
		}

		[HttpPut]
		public async Task<ActionResult> EditEvent(EditOrganizerDTO dto)
		{
			return HandleResult(await Mediator.Send(new Edit.Command { dto = dto }));
		}

		// [HttpDelete("{id}")]
		// public async Task<ActionResult> DeleteEvent(Guid id)
		// {
		// 	return HandleResult(await Mediator.Send(new Delete.Command { Id = id }));
		// }
	}
}