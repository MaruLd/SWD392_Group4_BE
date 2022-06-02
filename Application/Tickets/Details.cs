using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.Services;
using Application.Tickets.DTOs;
using AutoMapper;
using Domain;
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
			private readonly IMapper _mapper;

			public Handler(TicketService tickerService, IMapper mapper)
			{
				_ticketService = tickerService;
				_mapper = mapper;
			}


			public async Task<Result<TicketDTO>> Handle(Query request, CancellationToken cancellationToken)
			{
				var ticket = _ticketService.GetByID(request.Id);
				var ticketDto = _mapper.Map<TicketDTO>(ticket);
				return Result<TicketDTO>.Success(ticketDto);
			}
		}
	}
}