using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Controllers
{
	[ApiController]
	[Route("api/v{version:apiVersion}/[controller]")]
	[ApiVersion("1.0")]

	public class BaseApiController : ControllerBase
	{
		private IMediator _mediator;

		protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

		protected ActionResult HandleResult<T>(Result<T> result)
		{
			if (result == null) return NotFound();
			else if (result.IsSuccess && result.StatusCode == StatusCodes.Status204NoContent) return StatusCode(result.StatusCode);
			else if (result.IsSuccess && result.Value != null) return StatusCode(result.StatusCode, result.Value);
			else if (result.IsSuccess && result.Value == null) return NotFound(result.Error);
			return BadRequest(result.Error);


		}
	}
}