using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.Services;
using Application.Tickets.DTOs;
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

		public class Query : IRequest<Result<List<Ticket>>>
		{
			public ListTicketDTO dto { get; set; }
		}

		public class Handler : IRequestHandler<Query, Result<List<Ticket>>>
		{
			private readonly DataContext _context;
			private readonly TicketService _ticketService;

			public Handler(DataContext context, TicketService tickerService)
			{
				_context = context;
				_ticketService = tickerService;
			}

			public async Task<Result<List<Ticket>>> Handle(Query request, CancellationToken cancellationToken)
			{
				return Result<List<Ticket>>.Success(await _ticketService.Get(request.dto));
			}
		}
	}
}