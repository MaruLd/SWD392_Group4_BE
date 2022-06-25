using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Application.Core;
using Application.EventOrganizers.DTOs;
using Application.Events.DTOs;
using Application.EventUsers.DTOs;
using Application.Interfaces;
using Application.Services;
using Application.TicketUsers.DTOs;
using AutoMapper;
using Domain;
using Domain.Enums;
using FluentValidation;
using MediatR;
using Persistence;

namespace Application.EventOrganizers
{
	public class Create
	{
		public class Command : IRequest<Result<EventOrganizerDTO>> //Command do not return anything, but can return success or failure, return Unit also meant for nothing
		{
			public Guid eventId { get; set; }
			public CreateEventOrganizerDTO dto { get; set; }
		}

		public class Handler : IRequestHandler<Command, Result<EventOrganizerDTO>>
		{
			private readonly IUserAccessor _userAccessor;
			private readonly UserService _userService;
			private readonly EventService _eventService;
			private readonly OrganizerService _organizerService;
			private readonly EventOrganizerService _eventOrganizerService;
			private readonly EventUserService _eventUserService;
			private readonly IMapper _mapper;

			public Handler(EventService eventService, OrganizerService organizerService, EventOrganizerService eventOrganizerService, IMapper mapper, IUserAccessor userAccessor, UserService userService, EventUserService eventUserService)
			{
				this._eventService = eventService;
				this._organizerService = organizerService;
				this._eventOrganizerService = eventOrganizerService;
					this._eventUserService = eventUserService; ;
				_mapper = mapper;
				_userAccessor = userAccessor;
				this._userService = userService;
			}

			public async Task<Result<EventOrganizerDTO>> Handle(Command request, CancellationToken cancellationToken)
			{
				var user = await _userService.GetByEmail(_userAccessor.GetEmail());

				var e = await _eventService.GetByID(request.eventId);
				if (e == null) return Result<EventOrganizerDTO>.NotFound("Event Not Found!");

				var organizer = await _organizerService.GetByID(request.dto.OrganizerId);
				if (organizer == null) return Result<EventOrganizerDTO>.NotFound("Organizer Not Found!");

				var eventUser = await _eventUserService.GetByID(e.Id, user.Id);
				if (eventUser == null) return Result<EventOrganizerDTO>.Forbidden("You aren't in the event!");
				
				if (!eventUser.IsCreator())
				{
					return Result<EventOrganizerDTO>.Forbidden("You have no permission!");
				}

				var eventOrganizer = await _eventOrganizerService.GetByID(e.Id, organizer.Id);
				if (eventOrganizer != null) return Result<EventOrganizerDTO>.Failure("Event Organizer already existed!");

				var newEventOrganizer = new EventOrganizer() { EventId = e.Id, OrganizerId = organizer.Id };
				var result = await _eventOrganizerService.Insert(newEventOrganizer);

				if (!result) return Result<EventOrganizerDTO>.Failure("Failed to create event organizer");
				return Result<EventOrganizerDTO>.CreatedSuccess(_mapper.Map<EventOrganizerDTO>(newEventOrganizer));
			}
		}
	}
}