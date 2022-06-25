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
using Domain;
using MediatR;
using Persistence;

namespace Application.TicketUsers
{
	public class Details
	{
		public class Query : IRequest<Result<TicketUserDTO>>
		{
			public Guid ticketId { get; set; }
			public Guid userId { get; set; }
		}

		public class Handler : IRequestHandler<Query, Result<TicketUserDTO>>
		{
			private readonly EventService _eventService;
			private readonly EventUserService _eventUserService;
			private readonly IMapper _mapper;
			private readonly TicketService _ticketService;
			private readonly TicketUserService _ticketUserService;
			private readonly IUserAccessor _userAccessor;
			private readonly UserService _userService;

			public Handler(
				IMapper mapper,
				TicketService ticketService,
				TicketUserService ticketUserService,
				IUserAccessor userAccessor,
				UserService userService)
			{
				_mapper = mapper;
				this._ticketService = ticketService;
				this._ticketUserService = ticketUserService;
				this._userAccessor = userAccessor;
				this._userService = userService;
			}

			public async Task<Result<TicketUserDTO>> Handle(Query request, CancellationToken cancellationToken)
			{
				var user = await _userService.GetByEmail(_userAccessor.GetEmail());
				var t = await _ticketService.GetByID(request.ticketId);

				if (!(user.Id == request.userId)) // Check if getting self ticket user
				{
					if (t != null)
					{
						var eventUser = await _eventUserService.GetByID((Guid)t.EventId, user.Id);
						if (eventUser == null) return Result<TicketUserDTO>.Failure("No Permission");

						if (!eventUser.IsModerator())
						{
							// Ticket found but not a moderator
							return Result<TicketUserDTO>.Failure("No Permission");
						}
					}
					else
					{
						// TIcket not found and not a moderator
						return Result<TicketUserDTO>.Failure("No Permission");
					}
				}

				var result = await _ticketUserService.GetByID(t.Id, request.userId);
				if (result == null) return Result<TicketUserDTO>.NotFound("Ticket user not found!");

				return Result<TicketUserDTO>.Success(_mapper.Map<TicketUserDTO>(result));
			}
		}
	}
}