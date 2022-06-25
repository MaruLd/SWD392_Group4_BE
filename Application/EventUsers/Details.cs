using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.Events.DTOs;
using Application.EventUsers.DTOs;
using Application.Interfaces;
using Application.Services;
using AutoMapper;
using Domain;
using MediatR;
using Persistence;

namespace Application.EventUsers
{
	public class Details
	{
		public class Query : IRequest<Result<EventUserDTO>>
		{
			public Guid eventId { get; set; }
			public Guid userId { get; set; }
		}

		public class Handler : IRequestHandler<Query, Result<EventUserDTO>>
		{
			private readonly EventService _eventService;
			private readonly EventUserService _eventUserService;
			private readonly IUserAccessor _userAccessor;
			private readonly UserService _userService;
			private readonly IMapper _mapper;

			public Handler(IMapper mapper,
				  EventService eventService,
				  EventUserService eventUserService,
				  IUserAccessor userAccessor,
				  UserService userService)
			{
				_mapper = mapper;
				_eventService = eventService;
				_eventUserService = eventUserService;
				this._userAccessor = userAccessor;
				this._userService = userService;
			}

			public async Task<Result<EventUserDTO>> Handle(Query request, CancellationToken cancellationToken)
			{
				var user = await _userService.GetByEmail(_userAccessor.GetEmail());

				var e = await _eventService.GetByID(request.eventId, false);
				if (e == null) return Result<EventUserDTO>.Failure("Events not found!");

				var result = await _eventUserService.GetByID(request.eventId, user.Id);
				if (result == null) return Result<EventUserDTO>.NotFound("Not Found");

				if (user.Id != request.userId)
				{
					if (!result.IsModerator()) return Result<EventUserDTO>.Failure("No Permission");
				}
				if (result == null) return Result<EventUserDTO>.NotFound("Event user not found!");

				return Result<EventUserDTO>.Success(_mapper.Map<EventUserDTO>(result));
			}
		}
	}
}