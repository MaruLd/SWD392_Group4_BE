using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.Events.DTOs;
using Application.EventUsers.DTOs;
using Application.Services;
using Application.TicketUsers.DTOs;
using AutoMapper;
using Domain;
using MediatR;
using Persistence;

namespace Application.TicketUsers
{
	public class Details
	{
		public class Query : IRequest<Result<TicketUserDTO>>
		{
			public Guid ticketId { get; set; }
			public Guid userId { get; set; }
		}

		public class Handler : IRequestHandler<Query, Result<TicketUserDTO>>
		{
			private readonly EventService _eventService;
			private readonly EventUserService _eventUserService;
			private readonly IMapper _mapper;
			private readonly TicketService _ticketService;
			private readonly TicketUserService _ticketUserService;

			public Handler(IMapper mapper, TicketService ticketService, TicketUserService ticketUserService)
			{
				_mapper = mapper;
				this._ticketService = ticketService;
				this._ticketUserService = ticketUserService;
			}

			public async Task<Result<TicketUserDTO>> Handle(Query request, CancellationToken cancellationToken)
			{
				var t = await _ticketService.GetByID(request.ticketId);
				if (t == null) return Result<TicketUserDTO>.Failure("Ticket not found!");

				var result = await _ticketUserService.GetByID(t.Id, request.userId);
				if (result == null) return Result<TicketUserDTO>.NotFound("Ticket user not found!");

				return Result<TicketUserDTO>.Success(_mapper.Map<TicketUserDTO>(result));
			}
		}
	}
}