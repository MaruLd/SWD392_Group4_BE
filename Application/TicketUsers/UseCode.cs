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
	public class UseCode
	{
		public class Command : IRequest<Result<Unit>> //Command do not return anything, but can return success or failure, return Unit also meant for nothing
		{
			public Guid ticketId { get; set; }
			public string code { get; set; }
		}

		public class Handler : IRequestHandler<Command, Result<Unit>>
		{
			private readonly EventService _eventService;
			private readonly TicketService _ticketService;
			private readonly UserService _userService;
			private readonly TicketUserService _ticketUserService;
			private readonly EventUserService _eventUserService;
			private readonly IUserAccessor _userAccessor;
			private readonly EventCodeService _eventCodeService;
			private readonly IMapper _mapper;

			public Handler(
				EventService eventService,
				TicketService ticketService,
				UserService userService,
				TicketUserService ticketUserService,
				EventUserService eventUserService,
				IMapper mapper,
				IUserAccessor userAccessor,
				EventCodeService eventCodeService)
			{
				_mapper = mapper;
				_eventService = eventService;
				this._ticketService = ticketService;
				_userService = userService;
				this._ticketUserService = ticketUserService;
				this._eventUserService = eventUserService;
				_userAccessor = userAccessor;
				this._eventCodeService = eventCodeService;
			}

			public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
			{

				var ticket = await _ticketService.GetByID(request.ticketId);
				if (ticket == null) return Result<Unit>.NotFound("Ticket Not Found!");

				var e = ticket.Event;

				// var e = ticket.Event;
				// if (e.IsAbleToCheckin)
				var userId = _userAccessor.GetID();

				var ticketUser = await _ticketUserService.GetByID(ticket.Id, userId);
				if (ticketUser == null) return Result<Unit>.NotFound("You haven't buy this ticket");

				var eventCode = await _eventCodeService.GetByCode(request.code);
				if (eventCode == null) return Result<Unit>.Failure("This code is not valid!");

				if (eventCode.EventId != e.Id) return Result<Unit>.Failure("This code can't be apply to this event!");
				TicketUsersStateMachine sm = new TicketUsersStateMachine(ticketUser);

				if (e.IsAbleToCheckin() && ticketUser.State == TicketUserStateEnum.Idle)
				{
					sm.TriggerState(TicketUserStateEnum.CheckedIn);
				}
				else if (e.IsAbleToCheckout() && ticketUser.State == TicketUserStateEnum.CheckedOut)
				{
					sm.TriggerState(TicketUserStateEnum.CheckedOut);
				}

				var result = await _ticketUserService.Update(ticketUser);

				if (!result) return Result<Unit>.Failure("Failed to change ticket user!");
				return Result<Unit>.Success(Unit.Value);
			}
		}
	}
}