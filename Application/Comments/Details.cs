using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using MediatR;
using Persistence;

namespace Application.Comments
{
    public class Details 
    {
        public class Query : IRequest<Comment>{
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Comment>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Comment> Handle(Query request, CancellationToken cancellationToken)
            {
                var Comment = await _context.Comments.FindAsync(request.Id);

                if (Comment == null) throw new Exception("Comment not found");

                return Comment;
            }
        }
    }
}