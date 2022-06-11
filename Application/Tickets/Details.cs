using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.Interfaces;
using Application.Services;
using Application.Tickets.DTOs;
using AutoMapper;
using Domain;
using Domain.Enums;
using MediatR;
using Persistence;

namespace Application.Tickets
{
	public class Details
	{
		public class Query : IRequest<Result<TicketDTO>>
		{
			public Guid Id { get; set; }
		}

		public class Handler : IRequestHandler<Query, Result<TicketDTO>>
		{
			private readonly TicketService _ticketService;
			private readonly EventService _eventService;
			private readonly EventUserService _eventUserService;
			private readonly IMapper _mapper;
			private readonly IUserAccessor _userAccessor;

			public Handler(TicketService tickerService, EventService eventService, EventUserService eventUserService, IMapper mapper, IUserAccessor userAccessor)
			{
				_ticketService = tickerService;
				this._eventService = eventService;
				this._eventUserService = eventUserService;
				_mapper = mapper;
				this._userAccessor = userAccessor;
			}


			public async Task<Result<TicketDTO>> Handle(Query request, CancellationToken cancellationToken)
			{
				var ticket = await _ticketService.GetByID(request.Id);
				if (ticket == null) return Result<TicketDTO>.NotFound("Ticket Not Found!");

				var eventInDb = await _eventService.GetByID((Guid)ticket.EventId);
				if (eventInDb == null) return Result<TicketDTO>.Failure("Event not found!");

				var userId = _userAccessor.GetID();
				var eu = await _eventUserService.GetByID(eventInDb.Id, userId);

				if (eventInDb.State == EventStateEnum.Draft)
				{
					if (userId != null)
					{
						if (!eu.IsCreator()) return Result<TicketDTO>.Failure("No Permission");
					}
					return Result<TicketDTO>.Failure("No Permission");
				}

				var ticketDto = _mapper.Map<TicketDTO>(ticket);
				return Result<TicketDTO>.Success(ticketDto);
			}
		}
	}
}