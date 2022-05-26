using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using MediatR;
using Persistence;

namespace Application.Events
{
    public class Details 
    {
        public class Query : IRequest<Event>{
            public int Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Event>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Event> Handle(Query request, CancellationToken cancellationToken)
            {
                var Event = await _context.Event.FindAsync(request.Id);

                if (Event == null) throw new Exception("Event not found");

                return Event;
            }
        }
    }
}