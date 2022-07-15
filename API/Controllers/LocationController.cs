using System.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using API.DTOs;
using Microsoft.AspNetCore.Identity;
using Domain;
using Application.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Application.Users.DTOs;
using Application.Users;
using Application.Core;
using Application.TicketUsers.DTOs;
using Application.Tickets.DTOs;
using Application.Locations.DTOs;

namespace API.Controllers
{
	public class LocationController : BaseApiController
	{
		private readonly LocationService _locationService;

		public LocationController(LocationService locationService)
		{
			this._locationService = locationService;
		}

		/// <summary>
		/// Get Locations
		/// </summary>
		[HttpGet]
		public async Task<ActionResult<List<String>>> GetLocation([FromQuery] LocationQueryParams queryParams)
		{
			var locs = await _locationService.Get(queryParams);
			return locs.Select(l => l.Name).ToList();
		}

	}
}