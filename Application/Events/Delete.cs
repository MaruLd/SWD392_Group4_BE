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

namespace Application.Events
{
	public class Delete
	{
		public class Command : IRequest<Result<Unit>>
		{
			public Guid Id { get; set; }
		}

		public class Handler : IRequestHandler<Command, Result<Unit>>
		{
			private readonly UserService _userService;
			private readonly EventService _eventService;
			private readonly EventUserService _eventUserService;
			private readonly IUserAccessor _userAccessor;
			private readonly IMapper _mapper;

			public Handler(EventService eventService, EventUserService eventUserService, UserService userService, IMapper mapper, IUserAccessor userAccessor)
			{
				_userService = userService;
				_eventService = eventService;
				_eventUserService = eventUserService;
				_userAccessor = userAccessor;
				_mapper = mapper;
			}

			public async Task<Result<Unit>>
			Handle(Command request, CancellationToken cancellationToken)
			{
				var eventInDb = await _eventService.GetByID(request.Id);
				if (eventInDb == null) return Result<Unit>.Failure("Event not found!");

				var user = await _userService.GetByEmail(_userAccessor.GetEmail());
				var eventUser = await _eventUserService.GetByID(eventInDb.Id, user.Id);

				if (eventUser == null) return Result<Unit>.Forbidden("You aren't in the event!");
				if (!eventUser.IsCreator())
				{
					return Result<Unit>.Forbidden("You have no permission!");
				}

				if (eventInDb.State != EventStateEnum.Draft)
				{
					return Result<Unit>.Forbidden("You can only remove event in draft state!");
				}

				eventInDb.Status = StatusEnum.Unavailable;
				var result = await _eventService.Update(eventInDb);

				if (!result) return Result<Unit>.Failure("Failed to delete event");
				return Result<Unit>.NoContentSuccess(Unit.Value);
			}
		}
	}
}
