using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

		public class Query : IRequest<List<Ticket>>
		{
			public ListTicketDTO dto { get; set; }
		}

		public class Handler : IRequestHandler<Query, List<Ticket>>
		{
			private readonly DataContext _context;
			private readonly TicketService _ticketService;

			public Handler(DataContext context, TicketService tickerService)
			{
				_context = context;
				_ticketService = tickerService;
			}

			public async Task<List<Ticket>> Handle(Query request, CancellationToken cancellationToken)
			{
				var tickets = await _ticketService.Get(request.dto);
				if (tickets == null) throw new Exception("Ticket not found");
				return tickets;
			}
		}
	}
}