using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.Interfaces;
using Application.Services;
using Application.Tickets.DTOs;
using AutoMapper;
using Domain;
using FluentValidation;
using MediatR;
using Persistence;

namespace Application.Tickets
{
	public class Create
	{
		public class Command : IRequest<Result<Unit>>
		{
			public CreateTicketDTO dto { get; set; }
		}

		// public class CommandValidator : AbstractValidator<Command>
		// {
		//     public CommandValidator()
		//     {
		//         RuleFor(x => x.dto).SetValidator(new TicketValidator());
		//     }
		// }

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

			public async Task<Result<Unit>>
			Handle(Command request, CancellationToken cancellationToken)
			{
				var eventInDb = await _eventService.GetByID(request.dto.EventId);
				if (eventInDb == null) return Result<Unit>.Failure("Event not found!");

				var user = await _userService.GetByEmail(_userAccessor.GetEmail());
				var eventUser = await _eventUserService.GetByID(eventInDb.Id, user.Id);

				if (eventUser == null) return Result<Unit>.Failure("You aren't in the event!");

				var allowedRole = new List<EventUserType> { EventUserType.Admin, EventUserType.Manager };
				if (!allowedRole.Contains(eventUser.Type))
				{
					return Result<Unit>.Failure("You have no permission!");
				}

				var ticket = _mapper.Map<Ticket>(request.dto);

				var result = await _ticketService.Insert(ticket);
				if (!result) return Result<Unit>.Failure("Failed to create ticket");

				return Result<Unit>.Success(Unit.Value);
			}
		}
	}
}
