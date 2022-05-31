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
		public class Query : IRequest<Result<List<Event>>>
		{
			public ListEventParams dto { get; set; }
		}

		public class Handler : IRequestHandler<Query, Result<List<Event>>>
		{
			private readonly EventService _eventService;

			public Handler(EventService eventService)
			{
				_eventService = eventService;
			}

			public async Task<Result<List<Event>>> Handle(Query request, CancellationToken cancellationToken)
			{
				var res = await _eventService.Get(request.dto);
				return Result<List<Event>>.Success(res);
			}
		}
	}
}