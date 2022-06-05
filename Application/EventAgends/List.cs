using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.EventAgendas.DTOs;
using Application.Events.DTOs;
using Application.Services;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Repositories;

namespace Application.EventAgendas
{
	public class List
	{
		public class Query : IRequest<Result<List<EventAgendaDTO>>>
		{
			public Guid eventId { get; set; }
			public EventAgendaParams queryParams { get; set; }
		}

		public class Handler : IRequestHandler<Query, Result<List<EventAgendaDTO>>>
		{
			private readonly EventService _eventService;
			private readonly EventAgendaService _eventAgendaService;
			private readonly IMapper _mapper;

			public Handler(IMapper mapper, EventService eventService, EventAgendaService eventAgendaService)
			{
				_mapper = mapper;
				_eventService = eventService;
				this._eventAgendaService = eventAgendaService;
			}

			public async Task<Result<List<EventAgendaDTO>>> Handle(Query request, CancellationToken cancellationToken)
			{
				var e = await _eventService.GetByID(request.eventId);
				if (e == null) return Result<List<EventAgendaDTO>>.Failure("Event not found!");

				var eventAgendasDto = await _eventAgendaService.GetAllByEventId(request.eventId);
				return Result<List<EventAgendaDTO>>.Success(eventAgendasDto);
			}
		}
	}
}