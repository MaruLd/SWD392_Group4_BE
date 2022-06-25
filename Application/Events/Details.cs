using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.Events.DTOs;
using Application.Services;
using AutoMapper;
using Domain;
using MediatR;
using Persistence;

namespace Application.Events
{
	public class Details
	{
		public class Query : IRequest<Result<EventDTO>>
		{
			public Guid Id { get; set; }
		}

		public class Handler : IRequestHandler<Query, Result<EventDTO>>
		{
			private readonly EventService _eventService;
			private readonly IMapper _mapper;

			public Handler(EventService eventService, IMapper mapper)
			{
				;
				_eventService = eventService;
				_mapper = mapper;
			}

			public async Task<Result<EventDTO>> Handle(Query request, CancellationToken cancellationToken)
			{
				var e = await _eventService.GetByID(request.Id);
				if (e == null) return Result<EventDTO>.Failure("Event not found!");
				var dto = _mapper.Map<EventDTO>(e);
				return Result<EventDTO>.Success(dto);
			}
		}
	}
}