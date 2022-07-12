using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.Interfaces;
using Application.Services;
using Application.TicketUsers.DTOs;
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
			public Guid ticketId { get; set; }
		}


		public class Handler : IRequestHandler<Command, Result<Unit>>
		{
			private readonly TicketService _ticketService;
			private readonly UserService _userService;
			private readonly EventService _eventService;
			private readonly TicketUserService _ticketUserService;
			private readonly EventUserService _eventUserService;
			private readonly IUserAccessor _userAccessor;
			private readonly IMapper _mapper;

			public Handler(TicketService ticketService,
					EventService eventService,
					EventUserService eventUserService,
					TicketUserService ticketUserService,
					UserService userService,
					IMapper mapper,
					IUserAccessor userAccessor)
			{
				_userService = userService;
				_ticketService = ticketService;
				_eventService = eventService;
				_eventUserService = eventUserService;
				this._ticketUserService = ticketUserService;
				_userAccessor = userAccessor;
				_mapper = mapper;
			}
			public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
			{
				var ticket = await _ticketService.GetByID(request.ticketId);
				if (ticket == null) return Result<Unit>.NotFound("Ticket Not Found"); //If you DELETE something that doesn't exist, you should just return a 204 (even if the resource never existed). The client wanted the resource gone and it is gone. Returning a 404 is exposing internal processing that is unimportant to the client and will result in an unnecessary error condition.

				var eventInDb = await _eventService.GetByID((Guid)ticket.EventId);
				if (eventInDb == null) return Result<Unit>.AcceptedSuccess(Unit.Value); // Actually not needed but check anyways

				var user = await _userService.GetByEmail(_userAccessor.GetEmail());
				var eventUser = await _eventUserService.GetByID(eventInDb.Id, user.Id);

				if (eventUser == null) return Result<Unit>.Forbidden("You have no permission!");
				if (!eventUser.IsCreator())
				{
					return Result<Unit>.Forbidden("You have no permission!");
				}

				var users = await _ticketUserService.Get(request.ticketId);
				if (users.Count > 0) return Result<Unit>.Failure("Can't delete ticket that already been bought!");

				ticket.Status = StatusEnum.Unavailable;
				var result = await _eventService.Update(eventInDb);

				if (!result) return Result<Unit>.Failure("Failed to delete the ticket");
				return Result<Unit>.NoContentSuccess(Unit.Value);
			}
		}
	}
}