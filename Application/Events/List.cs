using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.Events.DTOs;
using Application.Services;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Params;
using Persistence.Repositories;

namespace Application.Events
{
	public class List
	{

		public class Query : IRequest<Result<List<EventDTO1>>>
		{
			public ListEventDTO dto { get; set; }
		}

		public class Handler : IRequestHandler<Query, Result<List<EventDTO1>>>
		{
			private readonly DataContext _context;
			private readonly EventService _eventService;

			public Handler(DataContext context, EventService eventService)
			{
				_context = context;
				_eventService = eventService;
			}

			public async Task<Result<List<EventDTO1>>> Handle(Query request, CancellationToken cancellationToken)
			{
				return Result<List<EventDTO1>>.Success(await _eventService.Get(request.dto));
			}
		}
	}
}