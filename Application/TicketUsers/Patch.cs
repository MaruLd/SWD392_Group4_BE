// using System;
// using System.Collections.Generic;
// using System.Data.Entity;
// using System.Linq;
// using System.Security.Claims;
// using System.Threading.Tasks;
// using Application.Core;
// using Application.Events.DTOs;
// using Application.EventUsers.DTOs;
// using Application.Interfaces;
// using Application.Services;
// using Application.TicketUsers.DTOs;
// using AutoMapper;
// using Domain;
// using Domain.Enums;
// using FluentValidation;
// using MediatR;
// using Persistence;

// namespace Application.TicketUsers
// {
// 	public class Patch
// 	{
// 		public class Command : IRequest<Result<TicketUserDTO>> //Command do not return anything, but can return success or failure, return Unit also meant for nothing
// 		{
// 			public Guid ticketId { get; set; }
// 			public Guid userId { get; set; }
// 			public TicketUserStateEnum ticketUserStateEnum { get; set; }
// 		}

// 		public class Handler : IRequestHandler<Command, Result<TicketUserDTO>>
// 		{
// 			private readonly EventService _eventService;
// 			private readonly TicketService _ticketService;
// 			private readonly UserService _userService;
// 			private readonly TicketUserService _ticketUserService;
// 			private readonly EventUserService _eventUserService;
// 			private readonly IUserAccessor _userAccessor;
// 			private readonly IMapper _mapper;

// 			public Handler(
// 				EventService eventService,
// 				TicketService ticketService,
// 				UserService userService,
// 				TicketUserService ticketUserService,
// 				EventUserService eventUserService
// 				IMapper mapper,
// 				IUserAccessor userAccessor)
// 			{
// 				_mapper = mapper;
// 				_eventService = eventService;
// 				this._ticketService = ticketService;
// 				_userService = userService;
// 				this._ticketUserService = ticketUserService;
// 				this._eventUserService = eventUserService;
// 				_userAccessor = userAccessor;
// 			}

// 			public async Task<Result<TicketUserDTO>> Handle(Command request, CancellationToken cancellationToken)
// 			{
// 				var user = await _userService.GetByEmail(_userAccessor.GetEmail());

// 				var ticket = await _ticketService.GetByID(request.ticketId);
// 				if (ticket == null) return Result<TicketUserDTO>.NotFound("Ticket Not Found!");

// 				var e = ticket.Event;
// 				if (e.)
				
// 				var userDst = await _userService.GetByID(request.userId);
// 				if (userDst == null) return Result<TicketUserDTO>.NotFound("User Not Found!");

// 				var ticketUser = await _ticketUserService.GetByID(ticket.Id, user.Id);
// 				if (ticketUser == null) return Result<TicketUserDTO>.NotFound("Ticket User Not Found!")

// 				var eventUser = await _eventUserService.GetByID((Guid)ticket.EventId, user.Id);
// 				if (eventUser == null)  return Result<TicketUserDTO>.Failure("You are not in the event");

// 				if (!eventUser.IsModerator())
// 				{
// 					return Result<TicketUserDTO>.Forbidden("You have no permission!");
// 				}

// 				TicketUsersStateMachine sm = new TicketUsersStateMachine(ticketUser);
// 				try
// 				{
// 					ticketUser = sm.CheckIn();
// 				}
// 				catch (InvalidOperationException err)
// 				{
//  					return Result<TicketUserDTO>.Failure("Invalid State Change!");
// 				}


// 				var usersCount = (await _ticketUserService.Get(ticket.Id)).Count();
// 				if (usersCount >= ticket.Quantity) return Result<TicketUserDTO>.Failure("Ticket is out of stock!");

// 				var newTicketUser = new TicketUser() { TicketId = ticket.Id, UserId = userDst.Id };
// 				var result = await _ticketUserService.Insert(newTicketUser);

// 				if (!result) return Result<TicketUserDTO>.Failure("Failed to create ticket user");
// 				return Result<TicketUserDTO>.CreatedSuccess(_mapper.Map<TicketUserDTO>(newTicketUser));
// 			}
// 		}
// 	}
// }