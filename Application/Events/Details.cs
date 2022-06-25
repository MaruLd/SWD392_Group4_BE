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
		public class Query : IRequest<Result<DetailEventDTO>>
		{
			public Guid Id { get; set; }
		}

		public class Handler : IRequestHandler<Query, Result<DetailEventDTO>>
		{
			private readonly EventService _eventService;
			private readonly IMapper _mapper;

			public Handler(EventService eventService, IMapper mapper)
			{
				;
				_eventService = eventService;
				_mapper = mapper;
			}

			public async Task<Result<DetailEventDTO>> Handle(Query request, CancellationToken cancellationToken)
			{
				var e = await _eventService.GetByID(request.Id);
				if (e == null) return Result<DetailEventDTO>.Failure("Event not found!");
				var dto = _mapper.Map<DetailEventDTO>(e);
				return Result<DetailEventDTO>.Success(dto);
			}
		}
	}
}