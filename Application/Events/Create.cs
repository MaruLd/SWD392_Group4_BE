using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Events.DTOs;
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
			public CreateEventDTO CreateEventDTO { get; set; }

		}

		public class CommandValidator : AbstractValidator<Command>
		{
			public CommandValidator()
			{
				RuleFor(x => x.CreateEventDTO).SetValidator(new CreateEventDTOValidator());

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
				Event e = _mapper.Map<Event>(request.CreateEventDTO);
				_context.Event.Add(e);

				await _context.SaveChangesAsync();

				return Unit.Value;
			}
		}
	}
}