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
	public class ListSelfTickets
	{

		public class Query : IRequest<Result<List<SelfTicketDTO>>>
		{
			public Guid userId { get; set; }
			public TickerUserSelfQueryParams queryParams { get; set; }
		}

		public class Handler : IRequestHandler<Query, Result<List<SelfTicketDTO>>>
		{
			private readonly TicketUserService _ticketUserService;
			private readonly IMapper _mapper;
			private readonly IHttpContextAccessor _httpContextAccessor;

			public Handler(TicketUserService ticketUserService, IMapper mapper, IHttpContextAccessor httpContextAccessor)
			{
				this._ticketUserService = ticketUserService;
				_mapper = mapper;
				this._httpContextAccessor = httpContextAccessor;
			}

			public async Task<Result<List<SelfTicketDTO>>> Handle(Query request, CancellationToken cancellationToken)
			{
				var res = await _ticketUserService.GetTicketsFromUser(request.userId, request.queryParams);
				_httpContextAccessor.HttpContext.Response.AddPaginationHeader<TicketUser>(res);
				return Result<List<SelfTicketDTO>>.Success(_mapper.Map<List<SelfTicketDTO>>(res));
			}
		}
	}
}