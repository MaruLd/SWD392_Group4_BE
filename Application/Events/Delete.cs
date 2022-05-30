using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Domain;
using MediatR;
using Persistence;

namespace Application.Events
{
    public class Delete
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Result<Unit>>
            Handle(Command request, CancellationToken cancellationToken)
            {
                var Event = await _context.Event.FindAsync(request.Id);

                _context.Remove (Event);

                var result = await _context.SaveChangesAsync()>0;
                if (!result) return Result<Unit>.Failure("Failed to delete event");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
