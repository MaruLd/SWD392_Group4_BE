using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
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
using static Application.Core.RedisConnection;

namespace Application.TicketUsers
{
	public class UseCode
	{
		public class Command : IRequest<Result<String>> //Command do not return anything, but can return success or failure, return Unit also meant for nothing
		{
			public Guid ticketId { get; set; }
			public string code { get; set; }
		}

		public class Handler : IRequestHandler<Command, Result<String>>
		{
			private readonly EventService _eventService;
			private readonly TicketService _ticketService;
			private readonly UserService _userService;
			private readonly TicketUserService _ticketUserService;
			private readonly EventUserService _eventUserService;
			private readonly IUserAccessor _userAccessor;
			private readonly EventCodeService _eventCodeService;
			private readonly RedisConnection _redisConnection;
			private readonly IMapper _mapper;

			public Handler(
				EventService eventService,
				TicketService ticketService,
				UserService userService,
				TicketUserService ticketUserService,
				EventUserService eventUserService,
				IMapper mapper,
				IUserAccessor userAccessor,
				EventCodeService eventCodeService,
				RedisConnection redisConnection)
			{
				_mapper = mapper;
				_eventService = eventService;
				this._ticketService = ticketService;
				_userService = userService;
				this._ticketUserService = ticketUserService;
				this._eventUserService = eventUserService;
				_userAccessor = userAccessor;
				this._eventCodeService = eventCodeService;
				this._redisConnection = redisConnection;
			}

			public async Task<Result<String>> Handle(Command request, CancellationToken cancellationToken)
			{

				var ticket = await _ticketService.GetByID(request.ticketId);
				if (ticket == null) return Result<String>.NotFound("Ticket Not Found!");

				var e = ticket.Event;

				// var e = ticket.Event;
				// if (e.IsAbleToCheckin)
				var userId = _userAccessor.GetID();

				var ticketUser = await _ticketUserService.GetByID(ticket.Id, userId);
				if (ticketUser == null) return Result<String>.NotFound("You haven't buy this ticket");

				var eventCode = await _eventCodeService.GetByCode(request.code);
				if (eventCode == null) return Result<String>.Failure("This code is not valid!");

				if (eventCode.EventId != e.Id) return Result<String>.Failure("This code can't be apply to this event!");

				TicketUsersStateMachine sm = new TicketUsersStateMachine(ticketUser);

				var message = "";
				if (e.IsAbleToCheckin() && ticketUser.State == TicketUserStateEnum.Idle)
				{
					sm.TriggerState(TicketUserStateEnum.CheckedIn);
					message = "Checkin";
				}
				else if (e.IsAbleToCheckout() && ticketUser.State == TicketUserStateEnum.CheckedIn)
				{
					sm.TriggerState(TicketUserStateEnum.CheckedOut);

					var user = ticketUser.User;
					var baseBonus = ticketUser.Ticket.Cost;

					if (e.StartTime < ticketUser.CheckedInDate)
					{
						var lossPercentage =
												(e.EndTime - ticketUser.CheckedInDate)
												/*===================================*/ /
													   (e.EndTime - e.StartTime);


						baseBonus = (float)(baseBonus * e.MultiplierFactor * lossPercentage);
					}
					else
					{
						baseBonus = (float)(baseBonus * e.MultiplierFactor);
					}

					user.Bean += baseBonus;

					_redisConnection.AddToQueue("UserUpdate", user);
					message = "Checkout";
					// await _userService.Update(user);
				}
				else
				{
					return Result<String>.Failure("Event or Ticket is not ready for checkin or checkout!");
				}

				var result = await _ticketUserService.Update(ticketUser);

				if (!result) return Result<String>.Failure("Failed to change ticket user!");
				return Result<String>.Success(message);
			}
		}
	}
}