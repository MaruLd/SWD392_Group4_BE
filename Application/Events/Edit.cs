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

namespace Application.Events
{
	public class Edit
	{
		public class Command : IRequest<Result<Unit>>
		{
			public EditEventDTO dto { get; set; }
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
			private readonly UserService _userService;
			private readonly EventService _eventService;
			private readonly EventUserService _eventUserService;
			private readonly IUserAccessor _userAccessor;
			private readonly IMapper _mapper;

			public Handler(EventService eventService, EventUserService eventUserService, UserService userService, IMapper mapper, IUserAccessor userAccessor)
			{
				_userService = userService;
				_eventService = eventService;
				_eventUserService = eventUserService;
				_userAccessor = userAccessor;
				_mapper = mapper;
			}

			public async Task<Result<Unit>>
			Handle(Command request, CancellationToken cancellationToken)
			{
				var eventInDb = await _eventService.GetByID(request.dto.Id);
				if (eventInDb == null) return Result<Unit>.Failure("Event not found!");

				var user = await _userService.GetByEmail(_userAccessor.GetEmail());
				var eventUser = await _eventUserService.GetByID(eventInDb.Id, user.Id);

				if (eventUser == null) return Result<Unit>.Forbidden("You aren't in the event!");

				if (!eventUser.IsModerator())
				{
					return Result<Unit>.Forbidden("You have no permission!");
				}

				if (!eventInDb.IsAbleToEdit())
				{
					return Result<Unit>.Forbidden("You can't no longer edit this event!");
				}

				_mapper.Map(request.dto, eventInDb);
				var result = await _eventService.Update(eventInDb);

				if (!result) return Result<Unit>.Failure("Failed to update event");

				return Result<Unit>.NoContentSuccess(Unit.Value);
			}
		}
	}
}
