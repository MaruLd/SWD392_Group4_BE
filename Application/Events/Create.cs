using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Application.Core;
using Application.Events.DTOs;
using Application.Interfaces;
using Application.Services;
using AutoMapper;
using Domain;
using FluentValidation;
using MediatR;
using Persistence;

namespace Application.Events
{
	public class Create
	{
		public class Command : IRequest<Result<EventDTO>> //Command do not return anything, but can return success or failure, return Unit also meant for nothing
		{
			public CreateEventDTO dto { get; set; }

		}

		public class Handler : IRequestHandler<Command, Result<EventDTO>>
		{
			private readonly EventService _eventService;
			private readonly UserService _userService;

			private readonly IUserAccessor _userAccessor;
			private readonly IMapper _mapper;

			public Handler(EventService eventService, UserService userService, IMapper mapper, IUserAccessor userAccessor)
			{
				_mapper = mapper;
				_eventService = eventService;
				_userService = userService;
				_userAccessor = userAccessor;
			}

			public async Task<Result<EventDTO>> Handle(Command request, CancellationToken cancellationToken)
			{
				var user = await _userService.GetByEmail(_userAccessor.GetEmail());
				try
				{
					var result = await _eventService.CreateEvent(request.dto, user.Id);
					if (result == null) return Result<EventDTO>.Failure("Failed to create event");

					return Result<EventDTO>.CreatedSuccess(_mapper.Map<EventDTO>(result)); //Unit.Value is nothing
				}
				catch
				{
					return Result<EventDTO>.Failure("Failed to create event");
				}
			}
		}
	}
}