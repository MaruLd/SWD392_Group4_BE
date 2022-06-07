using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Application.Core;
using Application.Events.DTOs;
using Application.EventUsers.DTOs;
using Application.Interfaces;
using Application.Services;
using Application.TicketUsers.DTOs;
using AutoMapper;
using Domain;
using Domain.Enums;
using FluentValidation;
using MediatR;
using Persistence;

namespace Application.Events
{
	public class Patch
	{
		public class Command : IRequest<Result<Unit>>
		{
			public Guid eventId { get; set; }
			public EventTriggerEnum eventTriggerEnum { get; set; }
		}

		public class Handler : IRequestHandler<Command, Result<Unit>>
		{
			private readonly EventService _eventService;
			private readonly TicketService _ticketService;
			private readonly UserService _userService;
			private readonly TicketUserService _ticketUserService;
			private readonly EventUserService _eventUserService;
			private readonly IUserAccessor _userAccessor;
			private readonly IMapper _mapper;

			public Handler(
				EventService eventService,
				TicketService ticketService,
				UserService userService,
				TicketUserService ticketUserService,
				EventUserService eventUserService,
				IMapper mapper,
				IUserAccessor userAccessor)
			{
				_mapper = mapper;
				_eventService = eventService;
				this._ticketService = ticketService;
				_userService = userService;
				this._ticketUserService = ticketUserService;
				this._eventUserService = eventUserService;
				_userAccessor = userAccessor;
			}

			public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
			{
				var user = await _userService.GetByEmail(_userAccessor.GetEmail());

				var e = await _eventService.GetByID(request.eventId);
				if (e == null) return Result<Unit>.NotFound("Event Not Found!");

				var eventUser = await _eventUserService.GetByID((Guid)e.Id, user.Id);
				if (eventUser == null) return Result<Unit>.Failure("You are not in the event");

				if (!eventUser.IsModerator())
				{
					return Result<Unit>.Failure("You have no permission!");
				}

				try
				{
					e.TriggerState((Domain.EventTriggerEnum)request.eventTriggerEnum);
				}
				catch
				{
					return Result<Unit>.Failure("Invalid State Change!");
				}

				var result = await _eventService.Update(e);

				if (!result) return Result<Unit>.Failure("Failed to change State");
				return Result<Unit>.NoContentSuccess(Unit.Value);
			}
		}
	}
}