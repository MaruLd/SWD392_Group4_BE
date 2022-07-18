using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Application.Core;
using Application.Events.DTOs;
using Application.Events.StateMachine;
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
			public PatchEventDTO dto { get; set; }
		}

		public class Handler : IRequestHandler<Command, Result<Unit>>
		{
			private readonly EventService _eventService;
			private readonly TicketService _ticketService;
			private readonly UserService _userService;
			private readonly TicketUserService _ticketUserService;
			private readonly EventUserService _eventUserService;
			private readonly FirebaseService _firebaseService;
			private readonly IUserAccessor _userAccessor;
			private readonly IMapper _mapper;
			private readonly RedisConnection _redisConnection;

			public Handler(
				EventService eventService,
				TicketService ticketService,
				UserService userService,
				TicketUserService ticketUserService,
				EventUserService eventUserService,
				FirebaseService firebaseService,
				IMapper mapper,
				RedisConnection redisConnection,
				IUserAccessor userAccessor)
			{
				_mapper = mapper;
				this._redisConnection = redisConnection;
				_eventService = eventService;
				this._ticketService = ticketService;
				_userService = userService;
				this._ticketUserService = ticketUserService;
				this._eventUserService = eventUserService;
				this._firebaseService = firebaseService;
				_userAccessor = userAccessor;
			}

			public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
			{
				var user = await _userService.GetByEmail(_userAccessor.GetEmail());

				var e = await _eventService.GetByID(request.dto.eventId);
				if (e == null) return Result<Unit>.NotFound("Event Not Found!");

				var eventUser = await _eventUserService.GetByID((Guid)e.Id, user.Id);
				if (eventUser == null) return Result<Unit>.Failure("You are not in the event");

				if (!eventUser.IsCreator())
				{
					return Result<Unit>.Failure("You have no permission!");
				}

				try
				{
					EventStateMachine esm = new EventStateMachine(e, _redisConnection);
					e = esm.TriggerState((EventStateEnum)request.dto.eventStateEnum);

					// Remove All TicketUser If Event Is Draft Or Cancelled (Refund Implement Later)
					// if (e.State == EventStateEnum.Draft || e.State == EventStateEnum.Cancelled)
					// {
					// 	var tickets = await _ticketService.GetAllFromEvent(e.Id, true);
					// 	tickets.ForEach(t =>
					// 		t.TicketUsers.ToList().ForEach(async tu =>
					// 		{
					// 			await _ticketUserService.Remove(tu);
					// 		}));
					// }
				}
				catch (Exception ex)
				{
					Trace.WriteLine(ex.StackTrace);
					return Result<Unit>.Failure("Invalid State Change!");
				}

				var result = await _eventService.Update(e);

				if (!result) return Result<Unit>.Failure("Failed to change State");
				return Result<Unit>.NoContentSuccess(Unit.Value);
			}
		}
	}
}