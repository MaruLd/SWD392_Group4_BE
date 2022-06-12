using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.Interfaces;
using Application.Services;
using Application.Tickets.DTOs;
using AutoMapper;
using Domain;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Repositories;

namespace Application.Tickets
{
	public class List
	{

		public class Query : IRequest<Result<List<TicketDTO>>>
		{
			public TicketQueryParams queryParams { get; set; }
		}

		public class Handler : IRequestHandler<Query, Result<List<TicketDTO>>>
		{
			private readonly TicketService _ticketService;
			private readonly EventService _eventService;
			private readonly EventUserService _eventUserService;
			private readonly IMapper _mapper;
			private readonly IUserAccessor _userAccessor;
			private readonly IHttpContextAccessor _httpContextAccessor;

			public Handler(
				TicketService tickerService,
				EventService eventService,
				EventUserService eventUserService,
				IMapper mapper,
				IUserAccessor userAccessor,
				IHttpContextAccessor httpContextAccessor)
			{
				_ticketService = tickerService;
				this._eventService = eventService;
				this._eventUserService = eventUserService;
				_mapper = mapper;
				this._userAccessor = userAccessor;
				this._httpContextAccessor = httpContextAccessor;
			}

			public async Task<Result<List<TicketDTO>>> Handle(Query request, CancellationToken cancellationToken)
			{
				var eventInDb = await _eventService.GetByID(request.queryParams.EventId);
				if (eventInDb == null) return Result<List<TicketDTO>>.Failure("Event not found!");

				if (eventInDb.State == EventStateEnum.Draft)
				{
					var userId = _userAccessor.GetID();
					if (userId == null)
					{
						return Result<List<TicketDTO>>.Failure("No Permission");
					}
					var eu = await _eventUserService.GetByID(eventInDb.Id, userId);
					if (!eu.IsCreator()) return Result<List<TicketDTO>>.Failure("No Permission");
				}

				var res = await _ticketService.Get(request.queryParams);
				var ticketDtos = _mapper.Map<List<TicketDTO>>(res);
				_httpContextAccessor.HttpContext.Response.AddPaginationHeader<Ticket>(res);

				return Result<List<TicketDTO>>.Success(ticketDtos);
			}
		}
	}
}