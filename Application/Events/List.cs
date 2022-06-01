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
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Params;
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
			private readonly DataContext _context;
			private readonly IMapper _mapper;

			public Handler(IMapper mapper, EventService eventService, DataContext context)
			{
				_mapper = mapper;
				_eventService = eventService;
				_context = context;
			}

			public async Task<Result<List<EventDTO>>> Handle(Query request, CancellationToken cancellationToken)
			{
				var res = await _eventService.Get(request.queryParams);
				var eventDtos = _mapper.Map<List<EventDTO>>(res);
				return Result<List<EventDTO>>.Success(eventDtos);
			}
		}
	}

}