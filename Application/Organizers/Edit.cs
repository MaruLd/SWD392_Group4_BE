using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.Events;
using Application.Events.DTOs;
using Application.Interfaces;
using Application.Services;
using AutoMapper;
using Domain;
using FluentValidation;
using MediatR;
using Persistence;

namespace Application.Organizers
{
	public class Edit
	{
		public class Command : IRequest<Result<Unit>>
		{
			public EditOrganizerDTO dto { get; set; }
		}

		// public class CommandValidator : AbstractValidator<Command>
		// {
		//     public CommandValidator()
		//     {
		//         RuleFor(x => x.Event).SetValidator(new EventValidator());
		//     }
		// }

		public class Handler : IRequestHandler<Command, Result<Unit>>
		{
			private readonly IMapper _mapper;
			private readonly OrganizerService _organizerService;

			public Handler(IMapper mapper, OrganizerService organizerService)
			{
				_mapper = mapper;
				this._organizerService = organizerService;
			}

			public async Task<Result<Unit>>
			Handle(Command request, CancellationToken cancellationToken)
			{
				var organizerinDb = await _organizerService.GetByID(request.dto.Id);
				if (organizerinDb == null) return Result<Unit>.Failure("Organizer not found!");

				_mapper.Map(request.dto, organizerinDb);
				var result = await _organizerService.Update(organizerinDb);
				if (!result) return Result<Unit>.Failure("Failed to update organizer");

				return Result<Unit>.NoContentSuccess(Unit.Value);
			}
		}
	}
}
