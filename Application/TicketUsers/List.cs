using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.Events.DTOs;
using Application.EventUsers.DTOs;
using Application.Interfaces;
using Application.Services;
using Application.TicketUsers.DTOs;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Http;
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
			private readonly EventUserService _eventUserService;
			private readonly TicketUserService _ticketUserService;
			private readonly UserService _userService;
			private readonly IUserAccessor _userAccessor;
			private readonly IHttpContextAccessor _httpContextAccessor;

			public Handler(
				IMapper mapper,
				TicketService ticketService,
				EventUserService eventUserService,
				TicketUserService ticketUserService,
				UserService userService,
				IUserAccessor userAccessor,
				IHttpContextAccessor httpContextAccessor)
			{
				_mapper = mapper;
				this._ticketService = ticketService;
				this._eventUserService = eventUserService;
				this._ticketUserService = ticketUserService;
				this._userService = userService;
				this._userAccessor = userAccessor;
				this._httpContextAccessor = httpContextAccessor;
			}

			public async Task<Result<List<TicketUserDTO>>> Handle(Query request, CancellationToken cancellationToken)
			{
				var user = await _userService.GetByEmail(_userAccessor.GetEmail());
				var t = await _ticketService.GetByID(request.ticketId);
				if (t != null)
				{
					var eventUser = await _eventUserService.GetByID((Guid)t.EventId, user.Id);
					if (eventUser == null) return Result<List<TicketUserDTO>>.Failure("No Permission");

					if (!eventUser.IsModerator())
					{
						// Ticket found but not a moderator
						return Result<List<TicketUserDTO>>.Failure("No Permission");
					}
				}
				else
				{
					// TIcket not found and not a moderator
					return Result<List<TicketUserDTO>>.Failure("No Permission");
				}

				var res = await _ticketUserService.Get(t.Id, request.queryParams);
				_httpContextAccessor.HttpContext.Response.AddPaginationHeader<TicketUser>(res);
				
				return Result<List<TicketUserDTO>>.Success(_mapper.Map<List<TicketUserDTO>>(res));
			}
		}
	}

}