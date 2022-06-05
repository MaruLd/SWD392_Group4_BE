using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.Interfaces;
using Application.Services;
using Application.Posts.DTOs;
using AutoMapper;
using Domain;
using FluentValidation;
using MediatR;
using Persistence;
using Domain.Enums;

namespace Application.Posts
{
	public class Edit
	{
		public class Command : IRequest<Result<Unit>>
		{
			public Guid postId { get; set; }
			public EditPostDTO dto { get; set; }

		}

		public class Handler : IRequestHandler<Command, Result<Unit>>
		{
			private readonly EventService _eventService;
			private readonly TicketService _ticketService;
			private readonly UserService _userService;
			private readonly EventUserService _eventUserService;
			private readonly IUserAccessor _userAccessor;
			private readonly IMapper _mapper;

			public Handler(EventService eventService, TicketService ticketService, UserService userService, EventUserService eventUserService, IUserAccessor userAccessor, IMapper mapper)
			{
				this._eventService = eventService;
				this._ticketService = ticketService;
				this._userService = userService;
				this._eventUserService = eventUserService;
				this._userAccessor = userAccessor;
				this._mapper = mapper;
			}
			public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
			{
				var user = await _userService.GetByEmail(_userAccessor.GetEmail());
				var ticket = await _ticketService.GetByID(request.postId);

				if (ticket == null) return Result<Unit>.Failure("Ticket not found!");

				var eventInDb = await _eventService.GetByID(ticket.EventId.Value);
				if (eventInDb == null) return Result<Unit>.Failure("Event not found!");

				var eventUser = await _eventUserService.GetByID(ticket.EventId.Value, user.Id);
				if (eventUser == null) return Result<Unit>.Unauthorized("You aren't in the event!");

				var allowedRole = new List<EventUserTypeEnum> { EventUserTypeEnum.Admin, EventUserTypeEnum.Manager };
				if (!allowedRole.Contains(eventUser.Type))
				{
					return Result<Unit>.Failure("You have no permission!");
				}

				var newTicket = _mapper.Map<EditPostDTO, Ticket>(request.dto, ticket);

				var result = await _ticketService.Save();
				if (!result) return Result<Unit>.Failure("Failed to update ticket or no changes was made!");

				return Result<Unit>.Success(Unit.Value);
			}
		}
	}
}