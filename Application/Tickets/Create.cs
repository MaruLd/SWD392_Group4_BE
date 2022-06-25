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
using Domain.Enums;
using FluentValidation;
using MediatR;
using Persistence;

namespace Application.Tickets
{
	public class Create
	{
		public class Command : IRequest<Result<TicketDTO>>
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

		public class Handler : IRequestHandler<Command, Result<TicketDTO>>
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

			public async Task<Result<TicketDTO>>
			Handle(Command request, CancellationToken cancellationToken)
			{
				var eventInDb = await _eventService.GetByID(request.dto.EventId);
				if (eventInDb == null) return Result<TicketDTO>.Failure("Event not found!");

				var user = await _userService.GetByEmail(_userAccessor.GetEmail());
				var eventUser = await _eventUserService.GetByID(eventInDb.Id, user.Id);

				if (eventUser == null) return Result<TicketDTO>.Forbidden("You aren't in the event!");
				if (!eventUser.IsCreator())
				{
					return Result<TicketDTO>.Forbidden("You have no permission!");
				}

				if (!eventInDb.IsAbleToEdit())
				{
					return Result<TicketDTO>.Forbidden("You can't no longer create ticket for this event!");
				}

				var ticket = _mapper.Map<Ticket>(request.dto);

				var result = await _ticketService.Insert(ticket);
				if (!result) return Result<TicketDTO>.Failure("Failed to create ticket");

				return Result<TicketDTO>.CreatedSuccess(_mapper.Map<TicketDTO>(ticket));
			}
		}
	}
}
