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
		public class Query : IRequest<Result<DetailTicketDTO>>
		{
			public Guid Id { get; set; }
		}

		public class Handler : IRequestHandler<Query, Result<DetailTicketDTO>>
		{
			private readonly TicketService _ticketService;
			private readonly IMapper _mapper;

			public Handler(TicketService ticketService, IMapper mapper)
			{
				_ticketService = ticketService;
				_mapper = mapper;
			}


			public async Task<Result<DetailTicketDTO>> Handle(Query request, CancellationToken cancellationToken)
			{
				var ticket = await _ticketService.GetByID(request.Id);
				var ticketDto = _mapper.Map<DetailTicketDTO>(ticket);
				return Result<DetailTicketDTO>.Success(ticketDto);
			}
		}
	}
}