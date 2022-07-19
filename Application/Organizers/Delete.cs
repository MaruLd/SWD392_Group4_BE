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
using Domain.Enums;
using FluentValidation;
using MediatR;
using Persistence;

namespace Application.Organizers
{
	public class Delete
	{
		public class Command : IRequest<Result<Unit>>
		{
			public Guid Id { get; set; }
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
			private readonly EventService _eventService;

			public Handler(IMapper mapper, OrganizerService organizerService, EventService eventService)
			{
				this._eventService = eventService;
				this._mapper = mapper;
				this._organizerService = organizerService;
			}

			public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
			{
				var organizerinDb = await _organizerService.GetByID(request.Id);
				if (organizerinDb == null) return Result<Unit>.NotFound("Organizer not found!");

				var events = await _eventService.Get(new EventQueryParams() { OrganizerName = organizerinDb.Name });

				if (events.Count() > 0) return Result<Unit>.Failure("Can't delete because organizer already have event!");
				organizerinDb.Status = StatusEnum.Unavailable;
				if (!await _organizerService.Update(organizerinDb)) return Result<Unit>.Failure("Failed to delete organizer");
				return Result<Unit>.NoContentSuccess(Unit.Value);
			}
		}
	}
}
