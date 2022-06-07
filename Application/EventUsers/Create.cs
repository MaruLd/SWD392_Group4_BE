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
using AutoMapper;
using Domain;
using Domain.Enums;
using FluentValidation;
using MediatR;
using Persistence;

namespace Application.EventUsers
{
	public class Create
	{
		public class Command : IRequest<Result<EventUserDTO>> //Command do not return anything, but can return success or failure, return Unit also meant for nothing
		{
			public Guid eventId { get; set; }
			public CreateEventUserDTO dto { get; set; }
		}

		public class Handler : IRequestHandler<Command, Result<EventUserDTO>>
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

			public async Task<Result<EventUserDTO>> Handle(Command request, CancellationToken cancellationToken)
			{
				var user = await _userService.GetByEmail(_userAccessor.GetEmail());
				var e = await _eventService.GetByID(request.eventId);
				if (e == null) return Result<EventUserDTO>.NotFound("Event Not Found!");


				var eUserCur = await _eventUserService.GetByID(e.Id, user.Id);
				if (eUserCur == null) return Result<EventUserDTO>.Failure("You are not in the event!");
				if (!eUserCur.IsModerator()) return Result<EventUserDTO>.Failure("No Permission");

				var dstUser = await _eventUserService.GetByID(e.Id, request.dto.UserId);
				if (dstUser != null) return Result<EventUserDTO>.Failure("User already in the event!");

				if (request.dto.Type > eUserCur.Type) return Result<EventUserDTO>.Failure("User's type is higher than you!");
				var newEu = new EventUser()
				{
					EventId = e.Id,
					UserId = request.dto.UserId,
					Type = request.dto.Type
				};

				var result = await _eventUserService.Insert(newEu);

				if (result == null) return Result<EventUserDTO>.Failure("Failed to create event user!");
				return Result<EventUserDTO>.CreatedSuccess(_mapper.Map<EventUserDTO>(newEu));
			}
		}
	}
}