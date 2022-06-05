using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using AutoMapper;
using Domain;
using FluentValidation;
using MediatR;
using Persistence;

namespace Application.Comments
{
    public class Edit
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Comment Comment { get; set; }
            
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var Comment = await _context.Comments.FindAsync(request.Comment.Id);

                if (Comment == null) return Result<Unit>.Failure("Comment not found");

                _mapper.Map(request.Comment, Comment);

                var result = await _context.SaveChangesAsync() > 0;

                if (!result) return Result<Unit>.Failure("Failed to update the comment");

                return Result<Unit>.NoContentSuccess(Unit.Value);
            }
        }
    }
}