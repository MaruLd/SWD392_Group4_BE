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

namespace Application.Tickets
{
    public class Edit
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Ticket Ticket { get; set; }
            
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Ticket).SetValidator(new TicketValidator());
    
            }

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
                var Ticket = await _context.Ticket.FindAsync(request.Ticket.Id);

                _mapper.Map(request.Ticket, Ticket);

                var result = await _context.SaveChangesAsync()>0;
                if(!result) return Result<Unit>.Failure("Failed to update ticket");
                return Result<Unit>.Success(Unit.Value);;
            }
        }
    }
}