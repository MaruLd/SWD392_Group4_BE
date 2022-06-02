using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Domain;
using FluentValidation;
using MediatR;
using Persistence;

namespace Application.Comments
{
    public class Edit
    {
        public class Command : IRequest
        {
            public Comment Comment { get; set; }
            
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var Comment = await _context.Comments.FindAsync(request.Comment.Id);

                _mapper.Map(request.Comment, Comment);

                await _context.SaveChangesAsync();
                
                return Unit.Value;
            }
        }
    }
}