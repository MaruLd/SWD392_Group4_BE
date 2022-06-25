using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Application.Core;
using Application.EventAgendas.DTOs;
using Application.Events.DTOs;
using Application.Interfaces;
using Application.Services;
using AutoMapper;
using Domain;
using Domain.Enums;
using FluentValidation;
using MediatR;
using Persistence;

namespace Application.EventAgendas
{
	public class Create
	{
		public class Command : IRequest<Result<EventAgendaDTO>> //Command do not return anything, but can return success or failure, return Unit also meant for nothing
		{
			public Guid eventId { get; set; }
			public CreateEventAgendaDTO dto { get; set; }
		}

		public class Handler : IRequestHandler<Command, Result<EventAgendaDTO>>
		{
			private readonly UserService _userService;
			private readonly EventUserService _eventUserService;
			private readonly EventService _eventService;
			private readonly EventAgendaService _eventAgendaService;
			private readonly IUserAccessor _userAccessor;
			private readonly IMapper _mapper;

			public Handler(UserService userService, EventUserService eventUserService, EventService eventService, EventAgendaService eventAgenda, IMapper mapper, IUserAccessor userAccessor)
			{
				_mapper = mapper;
				this._userService = userService;
				this._eventUserService = eventUserService;
				_eventService = eventService;
				this._eventAgendaService = eventAgenda;
				_userAccessor = userAccessor;
			}

			public async Task<Result<EventAgendaDTO>> Handle(Command request, CancellationToken cancellationToken)
			{
				var e = await _eventService.GetByID(request.eventId);
				if (e == null) return Result<EventAgendaDTO>.Failure("Event not found!");

				var user = await _userService.GetByEmail(_userAccessor.GetEmail());
				var eventUser = await _eventUserService.GetByID(e.Id, user.Id);
				if (eventUser == null) return Result<EventAgendaDTO>.Forbidden("You aren't in the event!");

				if (!eventUser.IsCreator())
				{
					return Result<EventAgendaDTO>.Forbidden("You have no permission!");
				}
				var ea = new EventAgenda() { EventId = request.eventId };
				_mapper.Map<CreateEventAgendaDTO, EventAgenda>(request.dto, ea);

				var result = await _eventAgendaService.Insert(ea);
				if (result == null) return Result<EventAgendaDTO>.Failure("Failed to create event agenda!");

				return Result<EventAgendaDTO>.CreatedSuccess(_mapper.Map<EventAgendaDTO>(ea)); //Unit.Value is nothing
			}
		}
	}
}