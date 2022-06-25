using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.Services;
using Application.Posts.DTOs;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Repositories;
using Application.Users.DTOs;
using Application.Tickets.DTOs;
using Application.TicketUsers.DTOs;
using Microsoft.AspNetCore.Http;

namespace Application.Users
{
	public class ListSelfEvents
	{

		public class Query : IRequest<Result<List<SelfEventDTO>>>
		{
			public Guid userId { get; set; }
			public EventSelfQueryParams queryParams { get; set; }
		}

		public class Handler : IRequestHandler<Query, Result<List<SelfEventDTO>>>
		{
			private readonly EventService _eventService;
			private readonly IMapper _mapper;
			private readonly IHttpContextAccessor _httpContextAccessor;

			public Handler(EventService eventService, IMapper mapper, IHttpContextAccessor httpContextAccessor)
			{
				this._eventService = eventService;
				_mapper = mapper;
				this._httpContextAccessor = httpContextAccessor;
			}

			public async Task<Result<List<SelfEventDTO>>> Handle(Query request, CancellationToken cancellationToken)
			{
				var res = await _eventService.GetSelfEvent(request.userId, request.queryParams);
				_httpContextAccessor.HttpContext.Response.AddPaginationHeader<Event>(res);
				return Result<List<SelfEventDTO>>.Success(_mapper.Map<List<SelfEventDTO>>(res));
			}
		}
	}
}