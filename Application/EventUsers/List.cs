using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.Events.DTOs;
using Application.EventUsers.DTOs;
using Application.Services;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Repositories;

namespace Application.EventUsers
{
	public class List
	{
		public class Query : IRequest<Result<List<EventUserDTO>>>
		{
			public Guid eventId { get; set; }
			public EventUserQueryParams queryParams { get; set; }
		}

		public class Handler : IRequestHandler<Query, Result<List<EventUserDTO>>>
		{
			private readonly EventService _eventService;
			private readonly EventUserService _eventUserService;
			private readonly IMapper _mapper;

			public Handler(IMapper mapper, EventService eventService, EventUserService eventUserService)
			{
				_mapper = mapper;
				_eventService = eventService;
				_eventUserService = eventUserService;
			}

			public async Task<Result<List<EventUserDTO>>> Handle(Query request, CancellationToken cancellationToken)
			{
				var e = await _eventService.GetByID(request.eventId);
				if (e == null) return Result<List<EventUserDTO>>.Failure("Events not found!");

				var res = await _eventUserService.Get(e.Id, request.queryParams);
				return Result<List<EventUserDTO>>.Success(_mapper.Map<List<EventUserDTO>>(res));
			}
		}
	}

}