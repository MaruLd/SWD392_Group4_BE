using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.EventAgendas.DTOs;
using Application.Events.DTOs;
using Application.Services;
using AutoMapper;
using Domain;
using MediatR;
using Persistence;

namespace Application.EventAgendas
{
	public class Details
	{
		public class Query : IRequest<Result<EventAgendaDTO>>
		{
			public Guid id { get; set; }
		}

		public class Handler : IRequestHandler<Query, Result<EventAgendaDTO>>
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

			public async Task<Result<EventAgendaDTO>> Handle(Query request, CancellationToken cancellationToken)
			{
				var eventAgendasDto = await _eventAgendaService.GetByID(request.id);
				if (eventAgendasDto == null) return Result<EventAgendaDTO>.NotFound("Not Found!");
				return Result<EventAgendaDTO>.Success(_mapper.Map<EventAgendaDTO>(eventAgendasDto));
			}
		}
	}
}