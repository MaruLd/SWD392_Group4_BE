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

namespace Application.TicketUsers
{
	public class Patch
	{
		public class Command : IRequest<Result<Unit>> //Command do not return anything, but can return success or failure, return Unit also meant for nothing
		{
			public Guid ticketId { get; set; }
			public Guid userId { get; set; }
			public TicketUserStateEnum ticketUserStateEnum { get; set; }
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

				var ticket = await _ticketService.GetByID(request.ticketId);
				if (ticket == null) return Result<Unit>.NotFound("Ticket Not Found!");

				// var e = ticket.Event;
				// if (e.IsAbleToCheckin)

				var userDst = await _userService.GetByID(request.userId);
				if (userDst == null) return Result<Unit>.NotFound("User Not Found!");

				var ticketUser = await _ticketUserService.GetByID(ticket.Id, user.Id);
				if (ticketUser == null) return Result<Unit>.NotFound("Ticket User Not Found!");

				var eventUser = await _eventUserService.GetByID((Guid)ticket.EventId, user.Id);
				if (eventUser == null) return Result<Unit>.Failure("You are not in the event");

				if (!eventUser.IsModerator())
				{
					return Result<Unit>.Forbidden("You have no permission!");
				}

				TicketUsersStateMachine sm = new TicketUsersStateMachine(ticketUser);
				try
				{
					ticketUser = sm.TriggerState(request.ticketUserStateEnum);
				}
				catch (InvalidOperationException err)
				{
					return Result<Unit>.Failure("Invalid State Change!");
				}


				var result = await _ticketUserService.Update(ticketUser);

				if (!result) return Result<Unit>.Failure("Failed to change Stater");
				return Result<Unit>.Success(Unit.Value);
			}
		}
	}
}