using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.Interfaces;
using Application.Services;
using AutoMapper;
using Domain;
using Domain.Enums;
using MediatR;
using Persistence;

namespace Application.Tickets
{
	public class Delete
	{
		public class Command : IRequest<Result<Unit>>
		{
			public Guid Id { get; set; }
		}

		private readonly TicketService _ticketService;
		private readonly UserService _userService;
		private readonly EventService _eventService;
		private readonly EventUserService _eventUserService;
		private readonly IUserAccessor _userAccessor;
		private readonly IMapper _mapper;

		public Delete(TicketService ticketService, EventService eventService, EventUserService eventUserService, UserService userService, IMapper mapper, IUserAccessor userAccessor)
		{
			_userService = userService;
			_ticketService = ticketService;
			_eventService = eventService;
			_eventUserService = eventUserService;
			_userAccessor = userAccessor;
			_mapper = mapper;
		}

		public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
		{
			var ticket = await _ticketService.GetByID(request.Id);
			if (ticket == null) return Result<Unit>.Failure("Ticket not found!");

			var eventInDb = await _eventService.GetByID((Guid)ticket.EventId);
			if (eventInDb == null) return Result<Unit>.Failure("Event not found!"); // Actually not needed but check anyways

			var user = await _userService.GetByEmail(_userAccessor.GetEmail());
			var eventUser = await _eventUserService.GetByID(eventInDb.Id, user.Id);

			if (eventUser == null) return Result<Unit>.Failure("You aren't in the event!");

			var allowedRole = new List<EventUserTypeEnum> { EventUserTypeEnum.Admin, EventUserTypeEnum.Manager };
			if (!allowedRole.Contains(eventUser.Type))
			{
				return Result<Unit>.Failure("You have no permission!");
			}


			var users = await _ticketService.GetAllUserByTicketId(ticket.Id);
			if (users.Count > 0) return Result<Unit>.Failure("Can't delete ticket that already been bought!");

			ticket.Status = StatusEnum.Unavailable;
			var result = await _eventService.Update(eventInDb);

			if (!result) return Result<Unit>.Failure("Failed to delete event");
			return Result<Unit>.NoContentSuccess(Unit.Value);
		}
	}
}