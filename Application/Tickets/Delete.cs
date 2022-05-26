using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using MediatR;
using Persistence;

namespace Application.Tickets
{
    public class Delete
    {
         public class Command : IRequest
        {
            public int Id { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                
               
                var Ticket = await _context.Ticket.FindAsync(request.Id);

                _context.Remove(Ticket);

                await _context.SaveChangesAsync();

                return Unit.Value;
            }
        }
    }
}