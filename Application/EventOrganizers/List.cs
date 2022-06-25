using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.EventOrganizers.DTOs;
using Application.Events.DTOs;
using Application.EventUsers.DTOs;
using Application.Services;
using Application.TicketUsers.DTOs;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Repositories;

namespace Application.EventOrganizers
{
	public class List
	{
		public class Query : IRequest<Result<List<EventOrganizerDTO>>>
		{
			public Guid eventId { get; set; }
		}

		public class Handler : IRequestHandler<Query, Result<List<EventOrganizerDTO>>>
		{

			private readonly IMapper _mapper;
			private readonly EventService _eventService;
			private readonly EventOrganizerService _eventOrganizerService;

			public Handler(IMapper mapper, EventService eventService, EventOrganizerService eventOrganizerService)
			{
				_mapper = mapper;
				this._eventService = eventService;
				this._eventOrganizerService = eventOrganizerService;
			}

			public async Task<Result<List<EventOrganizerDTO>>> Handle(Query request, CancellationToken cancellationToken)
			{
				var e = await _eventService.GetByID(request.eventId);
				if (e == null) return Result<List<EventOrganizerDTO>>.Failure("Ticket not found!");

				var res = await _eventOrganizerService.Get(e.Id);
				return Result<List<EventOrganizerDTO>>.Success(_mapper.Map<List<EventOrganizerDTO>>(res));
			}
		}
	}

}