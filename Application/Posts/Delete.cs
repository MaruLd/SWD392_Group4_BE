using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Domain;
using MediatR;
using Persistence;

namespace Application.Posts
{
    public class Delete
    {
         public class Command : IRequest<Result<Unit>>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Command,Result<Unit>>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                
               
                var Post = await _context.Post.FindAsync(request.Id);

                _context.Remove(Post);

                var result = await _context.SaveChangesAsync()>0;
                
                if (!result) return Result<Unit>.Failure("Failed to delete Post");

				return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}