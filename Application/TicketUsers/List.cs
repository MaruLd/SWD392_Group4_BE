using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.Events.DTOs;
using Application.EventUsers.DTOs;
using Application.Services;
using Application.TicketUsers.DTOs;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Repositories;

namespace Application.TicketUsers
{
	public class List
	{
		public class Query : IRequest<Result<List<TicketUserDTO>>>
		{
			public Guid ticketId { get; set; }
			public TicketUserQueryParams queryParams { get; set; }
		}

		public class Handler : IRequestHandler<Query, Result<List<TicketUserDTO>>>
		{

			private readonly IMapper _mapper;
			private readonly TicketService _ticketService;
			private readonly TicketUserService _ticketUserService;

			public Handler(IMapper mapper, TicketService ticketService, TicketUserService ticketUserService)
			{
				_mapper = mapper;
				this._ticketService = ticketService;
				this._ticketUserService = ticketUserService;
			}

			public async Task<Result<List<TicketUserDTO>>> Handle(Query request, CancellationToken cancellationToken)
			{
				var t = await _ticketService.GetByID(request.ticketId);
				if (t == null) return Result<List<TicketUserDTO>>.Failure("Ticket not found!");

				var res = await _ticketUserService.Get(t.Id, request.queryParams);
				return Result<List<TicketUserDTO>>.Success(_mapper.Map<List<TicketUserDTO>>(res));
			}
		}
	}

}