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

namespace Application.Users
{
	public class ListSelfTickets
	{

		public class Query : IRequest<Result<List<TicketDTO>>>
		{
			public Guid userId { get; set; }
			public TickerUserSelfQueryParams queryParams { get; set; }
		}

		public class Handler : IRequestHandler<Query, Result<List<TicketDTO>>>
		{
			private readonly TicketUserService _ticketUserService;
			private readonly IMapper _mapper;

			public Handler(TicketUserService ticketUserService, IMapper mapper)
			{
				this._ticketUserService = ticketUserService;
				_mapper = mapper;
			}

			public async Task<Result<List<TicketDTO>>> Handle(Query request, CancellationToken cancellationToken)
			{
				var res = await _ticketUserService.GetTicketsFromUser(request.userId, request.queryParams);
				var tickets = res.Select(tu => tu.Ticket);
				return Result<List<TicketDTO>>.Success(_mapper.Map<List<TicketDTO>>(tickets));
			}
		}
	}
}