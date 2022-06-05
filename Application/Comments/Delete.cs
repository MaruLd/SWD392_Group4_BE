using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Domain;
using Domain.Enums;
using MediatR;
using Persistence;

namespace Application.Comments
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

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var Comment = await _context.Comments.FindAsync(request.Id);

                if (Comment.Status == StatusEnum.Unavailable) return Result<Unit>.AcceptedSuccess(Unit.Value);

                Comment.Status = StatusEnum.Unavailable;

                var result = await _context.SaveChangesAsync() > 0;

                if (!result) return Result<Unit>.Failure("Failed to delete the comment");

                return Result<Unit>.AcceptedSuccess(Unit.Value);
            }
        }
    }
}