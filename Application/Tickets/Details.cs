using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using MediatR;
using Persistence;

namespace Application.Tickets
{
    public class Details 
    {
        public class Query : IRequest<Ticket>{
            public int Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Ticket>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Ticket> Handle(Query request, CancellationToken cancellationToken)
            {
                var ticket = await _context.Ticket.FindAsync(request.Id);

                if (ticket == null) throw new Exception("Ticket not found");

                return ticket;
            }
        }
    }
}