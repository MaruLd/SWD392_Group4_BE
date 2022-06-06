using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.Events.DTOs;
using Application.EventUsers.DTOs;
using Application.Services;
using AutoMapper;
using Domain;
using MediatR;
using Persistence;

namespace Application.TicketUsers
{
	public class Details
	{
		public class Query : IRequest<Result<EventUserDTO>>
		{
			public Guid eventId { get; set; }
			public Guid userId { get; set; }
		}

		public class Handler : IRequestHandler<Query, Result<EventUserDTO>>
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

			public async Task<Result<EventUserDTO>> Handle(Query request, CancellationToken cancellationToken)
			{
				var e = await _eventService.GetByID(request.eventId);
				if (e == null) return Result<EventUserDTO>.Failure("Events not found!");

				var result = await _eventUserService.GetByID(e.Id, request.userId);
				if (result == null) return Result<EventUserDTO>.NotFound("Event user not found!");

				return Result<EventUserDTO>.Success(_mapper.Map<EventUserDTO>(result));
			}
		}
	}
}