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

namespace Application.Users
{
	public class ListSelfTickets
	{

		public class Query : IRequest<Result<List<TicketUserDTO>>>
		{
			public Guid userId { get; set; }
			public TickerUserSelfQueryParams queryParams { get; set; }
		}

		public class Handler : IRequestHandler<Query, Result<List<TicketUserDTO>>>
		{
			private readonly TicketUserService _ticketUserService;
			private readonly IMapper _mapper;

			public Handler(TicketUserService ticketUserService, IMapper mapper)
			{
				this._ticketUserService = ticketUserService;
				_mapper = mapper;
			}

			public async Task<Result<List<TicketUserDTO>>> Handle(Query request, CancellationToken cancellationToken)
			{
				var res = await _ticketUserService.GetTicketsFromUser(request.userId, request.queryParams);
				return Result<List<TicketUserDTO>>.Success(_mapper.Map<List<TicketUserDTO>>(res));
			}
		}
	}
}