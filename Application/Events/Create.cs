using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Domain;
using FluentValidation;
using MediatR;
using Persistence;

namespace Application.Events
{
	public class Create
	{
		public class Command : IRequest
		{
			public Event Event { get; set; }

		}

		public class CommandValidator : AbstractValidator<Command>
		{
			public CommandValidator()
			{
				RuleFor(x => x.Event).SetValidator(new EventValidator());
			}

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
				Event e = _mapper.Map<Event>(request.Event);
				_context.Event.Add(e);

				await _context.SaveChangesAsync();

				return Unit.Value;
			}
		}
	}
}