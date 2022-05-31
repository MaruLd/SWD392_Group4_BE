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
			public Event dto { get; set; }
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
				// var res = await _eventService.Get(request.dto);
				// return Result<List<Event>>.Success(res);
				var Events = await _context.Event.ProjectTo<EventDTO>(_mapper.ConfigurationProvider).ToListAsync(cancellationToken);
        return Result<List<EventDTO>>.Success(Events);
			}
		}
	}
}