using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.Events;
using Application.Events.DTOs;
using Application.EventUsers.DTOs;
using Application.Interfaces;
using Application.Services;
using AutoMapper;
using Domain;
using Domain.Enums;
using FluentValidation;
using MediatR;
using Persistence;

namespace Application.EventUsers
{
	public class Edit
	{
		public class Command : IRequest<Result<Unit>> //Command do not return anything, but can return success or failure, return Unit also meant for nothing
		{
			public Guid eventId { get; set; }
			public EditEventUserDTO dto { get; set; }
		}

		public class Handler : IRequestHandler<Command, Result<Unit>>
		{
			private readonly EventService _eventService;
			private readonly UserService _userService;
			private readonly EventUserService _eventUserService;
			private readonly IUserAccessor _userAccessor;
			private readonly IMapper _mapper;

			public Handler(EventService eventService, UserService userService, EventUserService eventUserService, IMapper mapper, IUserAccessor userAccessor)
			{
				_mapper = mapper;
				_eventService = eventService;
				_userService = userService;
				this._eventUserService = eventUserService;
				_userAccessor = userAccessor;
			}

			public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
			{
				var user = await _userService.GetByEmail(_userAccessor.GetEmail());
				if (user.Id == request.dto.UserId) return Result<Unit>.Failure("You cannot edit yourself!");

				var e = await _eventService.GetByID(request.eventId);
				if (e == null) return Result<Unit>.NotFound("Event Not Found!");

				var eUserCur = await _eventUserService.GetByID(e.Id, user.Id);
				if (eUserCur == null) return Result<Unit>.Failure("You are not in the event!");
				if (!eUserCur.IsModerator()) return Result<Unit>.Failure("No Permission");

				var dstUser = await _eventUserService.GetByID(e.Id, request.dto.UserId);
				if (dstUser == null) return Result<Unit>.Failure("User is not in the event");
				if (dstUser.Type == eUserCur.Type) return Result<Unit>.Failure("User current role is as same as you!");
				if (request.dto.Type > eUserCur.Type) return Result<Unit>.Failure("User's new type is higher than you!");

				_mapper.Map<EditEventUserDTO, EventUser>(request.dto, dstUser);

				var result = await _eventUserService.Save();

				if (!result) return Result<Unit>.Failure("Failed to edit event user!");
				return Result<Unit>.NoContentSuccess(Unit.Value);
			}
		}
	}
}
