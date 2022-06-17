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
using AutoMapper.QueryableExtensions;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Repositories;

namespace Application.EventUsers
{
	public class List
	{
		public class Query : IRequest<Result<List<EventUserDTO>>>
		{
			public Guid eventId { get; set; }
			public EventUserQueryParams queryParams { get; set; }
		}

		public class Handler : IRequestHandler<Query, Result<List<EventUserDTO>>>
		{
			private readonly EventService _eventService;
			private readonly EventUserService _eventUserService;
			private readonly IUserAccessor _userAccessor;
			private readonly UserService _userService;
			private readonly IHttpContextAccessor _httpContextAccessor;
			private readonly IMapper _mapper;

			public Handler(IMapper mapper,
				  EventService eventService,
				  EventUserService eventUserService,
				  IUserAccessor userAccessor,
				  UserService userService,
				  IHttpContextAccessor httpContextAccessor)
			{
				_mapper = mapper;
				_eventService = eventService;
				_eventUserService = eventUserService;
				this._userAccessor = userAccessor;
				this._userService = userService;
				this._httpContextAccessor = httpContextAccessor;
			}

			public async Task<Result<List<EventUserDTO>>> Handle(Query request, CancellationToken cancellationToken)
			{
				var user = await _userService.GetByEmail(_userAccessor.GetEmail());

				var e = await _eventService.GetByID(request.eventId, false);
				if (e == null) return Result<List<EventUserDTO>>.Failure("Events not found!");

				var eventUser = await _eventUserService.GetByID(e.Id, user.Id);
				if (eventUser == null) return Result<List<EventUserDTO>>.Failure("No Permission");
				if (!eventUser.IsModerator()) return Result<List<EventUserDTO>>.Failure("No Permission");

				var res = await _eventUserService.Get(e.Id, request.queryParams);
				_httpContextAccessor.HttpContext.Response.AddPaginationHeader<EventUser>(res);

				return Result<List<EventUserDTO>>.Success(_mapper.Map<List<EventUserDTO>>(res));
			}
		}
	}

}