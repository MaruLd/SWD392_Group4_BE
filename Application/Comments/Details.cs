using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Domain;
using MediatR;
using Persistence;

namespace Application.Comments
{
    public class Details 
    {
        public class Query : IRequest<Result<Comment>>{
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<Comment>>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Result<Comment>> Handle(Query request, CancellationToken cancellationToken)
            {
                var Comment = await _context.Comments.FindAsync(request.Id);

                if (Comment == null) Result<Comment>.Failure("Comment not found");

                return Result<Comment>.Success(Comment);
            }
        }
    }
}