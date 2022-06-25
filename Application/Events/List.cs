using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.Events.DTOs;
using Application.Services;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Repositories;

namespace Application.Events
{
	public class List
	{
		public class Query : IRequest<Result<List<EventDTO>>>
		{
			public EventQueryParams queryParams { get; set; }
		}

		public class Handler : IRequestHandler<Query, Result<List<EventDTO>>>
		{
			private readonly EventService _eventService;
			private readonly IHttpContextAccessor _httpContextAccessor;
			private readonly IMapper _mapper;

			public Handler(IMapper mapper, EventService eventService, IHttpContextAccessor httpContextAccessor)
			{
				_mapper = mapper;
				_eventService = eventService;
				this._httpContextAccessor = httpContextAccessor;
			}

			public async Task<Result<List<EventDTO>>> Handle(Query request, CancellationToken cancellationToken)
			{
				var res = await _eventService.Get(request.queryParams);
				if (res == null) return Result<List<EventDTO>>.Failure("Events not found!");

				_httpContextAccessor.HttpContext.Response.AddPaginationHeader<Event>(res);

				var eventDtos = _mapper.Map<List<EventDTO>>(res);
				return Result<List<EventDTO>>.Success(eventDtos);
			}
		}
	}

}