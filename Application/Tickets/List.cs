using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
			public TicketParams ticketParams { get; set; }
		}

		public class Handler : IRequestHandler<Query, List<Ticket>>
		{
			private readonly DataContext _context;
			private readonly TicketRepository _ticketRepo;

			public Handler(DataContext context, TicketRepository ticketRepository)
			{
				_context = context;
				_ticketRepo = ticketRepository;
			}

			public async Task<List<Ticket>> Handle(Query request, CancellationToken cancellationToken)
			{
				var tickets = await _ticketRepo.Get(request.ticketParams);
				if (tickets == null) throw new Exception("Ticket not found");
				return tickets;
			}
		}
	}
}