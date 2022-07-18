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
	public class Create
	{
		public class Command : IRequest<Result<TicketUserDTO>> //Command do not return anything, but can return success or failure, return Unit also meant for nothing
		{
			public Guid ticketId { get; set; }
			public CreateTicketUserDTO dto { get; set; }
		}

		public class Handler : IRequestHandler<Command, Result<TicketUserDTO>>
		{
			private readonly EventService _eventService;
			private readonly TicketService _ticketService;
			private readonly UserService _userService;
			private readonly TicketUserService _ticketUserService;
			private readonly EventUserService _eventUserService;
			private readonly IUserAccessor _userAccessor;
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
				RedisConnection redisConnection)
			{
				_mapper = mapper;
				_eventService = eventService;
				this._ticketService = ticketService;
				_userService = userService;
				this._ticketUserService = ticketUserService;
				this._eventUserService = eventUserService;
				_userAccessor = userAccessor;
				this._redisConnection = redisConnection;
			}

			public async Task<Result<TicketUserDTO>> Handle(Command request, CancellationToken cancellationToken)
			{
				var user = await _userService.GetByEmail(_userAccessor.GetEmail());
				if (user.Id != request.dto.UserId) return Result<TicketUserDTO>.Failure("You can only buy ticket for yourself!");

				var ticket = await _ticketService.GetByID(request.ticketId);
				if (ticket == null) return Result<TicketUserDTO>.NotFound("Ticket Not Found!");

				if (!ticket.Event.IsAbleToBuyTicket()) return Result<TicketUserDTO>.Failure("Event currently not selling ticket!");

				var userDst = await _userService.GetByID(request.dto.UserId);
				if (userDst == null) return Result<TicketUserDTO>.NotFound("User Not Found!");

				var ticketUser = await _ticketUserService.GetByID(ticket.Id, request.dto.UserId);
				if (ticketUser != null) return Result<TicketUserDTO>.Failure("You already buy this!");

				var ticketFind = await _ticketUserService.GetTicketUserInEvent((Guid)ticket.EventId, request.dto.UserId);
				if (ticketFind != null) return Result<TicketUserDTO>.Failure("You already bought a ticket in this event!");

				var usersCount = (await _ticketUserService.Get(ticket.Id)).Count();
				if (usersCount >= ticket.Quantity) return Result<TicketUserDTO>.Failure("Ticket is out of stock!");

				if (user.Bean < ticket.Cost)
				{
					return Result<TicketUserDTO>.Failure("You don't have enough Bean!");
				}

				var newTicketUser = new TicketUser() { TicketId = ticket.Id, UserId = userDst.Id };

				var result = await _ticketUserService.Insert(newTicketUser);
				if (!result) return Result<TicketUserDTO>.Failure("Failed to create ticket user");

				user.Bean -= ticket.Cost;
				await _userService.Update(user);

				try
				{
					var eventUser = await _eventUserService.GetByID(ticket.EventId.Value, user.Id);
					if (eventUser == null)
					{
						eventUser = new EventUser() { EventId = ticket.EventId, UserId = userDst.Id, Type = EventUserTypeEnum.Student };
					}
					await _eventUserService.Insert(eventUser);
				}
				catch { }

				_redisConnection.AddToQueue("UserUpdate", user as User);

				var euData = new Dictionary<String, Guid>();
				euData.Add("eventId", ticket.EventId.Value);
				euData.Add("userId", user.Id);
				_redisConnection.AddToQueue("EventAddUser", euData);

				return Result<TicketUserDTO>.CreatedSuccess(_mapper.Map<TicketUserDTO>(newTicketUser));
			}
		}
	}
}