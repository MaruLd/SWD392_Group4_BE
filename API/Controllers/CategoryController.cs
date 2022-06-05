using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.EventCategories.DTOs;
using Application.Services;
using AutoMapper;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
	[Route("api/v{version:apiVersion}/event-category")]
	[ApiVersion("1.0")]
	public class EventCategoryController : BaseApiController
	{
		private readonly EventCategoryService _eventCategoryService;
		private IMapper _mapper;

		public EventCategoryController(EventCategoryService eventCategoryService, IMapper mapper)
		{
			this._eventCategoryService = eventCategoryService;
			_mapper = mapper;
		}

		[HttpGet]
		public async Task<ActionResult<List<EventCategoryDTO>>> GetCategories()
		{
			var result = await _eventCategoryService.GetAll();
			return Ok(_mapper.Map<List<EventCategoryDTO>>(result));
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<EventCategoryDTO>> GetCategory([FromQuery] int id)
		{
			var ecDto = await _eventCategoryService.GetByID(id);
			if (ecDto == null) return NotFound();
			return Ok(ecDto);
		}

		/// <summary>
		/// [Authorize]
		/// </summary>
		[Authorize(Roles = "Admin")]
		[HttpPost]
		public async Task<ActionResult<EventCategoryDTO>> CreateCategory([FromQuery] string categoryName)
		{
			EventCategory ec = new EventCategory() { Name = categoryName };
			var res = await _eventCategoryService.Insert(ec);
			if (!res) return BadRequest();
			return Ok(_mapper.Map<EventCategoryDTO>(ec));
		}

		/// <summary>
		/// [Authorize]
		/// </summary>
		[Authorize(Roles = "Admin")]
		[HttpPut()]
		public async Task<ActionResult> EditCategory(int id, string categoryName)
		{
			var cat = await _eventCategoryService.GetByID(id);
			if (cat == null) return NotFound("Category not found!");

			cat.Name = categoryName;
			return await _eventCategoryService.Update(_mapper.Map<EventCategory>(cat)) ? NoContent() : BadRequest();
		}

		/// <summary>
		/// [Authorize]
		/// </summary>
		[Authorize(Roles = "Admin")]
		[HttpDelete("{id}")]
		public async Task<ActionResult> DeleteCategory([FromBody] int id)
		{
			var cat = await _eventCategoryService.GetByID(id);
			if (cat == null) return NotFound("Category not found!");

			if (cat.Events.Count() > 0) return BadRequest("This category already have event!");

			return await _eventCategoryService.Delete(cat) ? NoContent() : BadRequest();
		}
	}
}