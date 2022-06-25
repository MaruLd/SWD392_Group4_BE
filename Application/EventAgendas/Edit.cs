using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.EventAgendas.DTOs;
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

namespace Application.EventAgendas
{
	public class Edit
	{
		public class Command : IRequest<Result<Unit>>
		{
			public EditEventAgendaDTO dto { get; set; }
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
			private readonly EventAgendaService _eventAgendaService;
			private readonly EventService _eventService;
			private readonly EventUserService _eventUserService;
			private readonly IUserAccessor _userAccessor;
			private readonly IMapper _mapper;

			public Handler(EventAgendaService eventAgendaService, EventService eventService, EventUserService eventUserService, UserService userService, IMapper mapper, IUserAccessor userAccessor)
			{
				_userService = userService;
				this._eventAgendaService = eventAgendaService;
				_eventService = eventService;
				_eventUserService = eventUserService;
				_userAccessor = userAccessor;
				_mapper = mapper;
			}

			public async Task<Result<Unit>>
			Handle(Command request, CancellationToken cancellationToken)
			{
				var agenda = await _eventAgendaService.GetByID(request.dto.Id);
				if (agenda == null) return Result<Unit>.Failure("Agenda not found!");

				var eventInDb = await _eventService.GetByID((Guid)agenda.EventId);
				if (eventInDb == null) return Result<Unit>.Failure("Event not found!");

				var user = await _userService.GetByEmail(_userAccessor.GetEmail());
				var eventUser = await _eventUserService.GetByID(eventInDb.Id, user.Id);

				if (eventUser == null) return Result<Unit>.Forbidden("You aren't in the event!");


				if (!eventUser.IsCreator())
				{
					return Result<Unit>.Forbidden("You have no permission!");
				}

				_mapper.Map(request.dto, agenda);
				var result = await _eventAgendaService.Update(agenda);

				if (!result) return Result<Unit>.Failure("Failed to update agenda");

				return Result<Unit>.NoContentSuccess(Unit.Value);
			}
		}
	}
}
