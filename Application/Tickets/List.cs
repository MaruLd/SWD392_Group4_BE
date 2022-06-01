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
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Params;
using Persistence.Repositories;

namespace Application.Tickets
{
	public class List
	{

		public class Query : IRequest<Result<List<TicketDTO>>>
		{
			public ListTicketDTO dto { get; set; }
		}

		public class Handler : IRequestHandler<Query, Result<List<TicketDTO>>>
		{
			private readonly DataContext _context;
			private readonly TicketService _ticketService;
			private readonly IMapper _mapper;

			public Handler(DataContext context, TicketService tickerService, IMapper mapper)
			{
				_context = context;
				_ticketService = tickerService;
				this._mapper = mapper;
			}

			public async Task<Result<List<TicketDTO>>> Handle(Query request, CancellationToken cancellationToken)
			{
				var res = await _ticketService.Get(request.dto);
				var ticketDtos = _mapper.Map<List<TicketDTO>>(res);
				return Result<List<TicketDTO>>.Success(ticketDtos);
			}
		}
	}
}