using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

		public class Query : IRequest<List<Event>>
		{
			public EventParams eventParams { get; set; }
		}

		public class Handler : IRequestHandler<Query, List<Event>>
		{
			private readonly DataContext _context;
			private readonly EventRepository _eventRepo;

			public Handler(DataContext context, EventRepository eventRepository)
			{
				_context = context;
				_eventRepo = eventRepository;
			}

			public async Task<List<Event>> Handle(Query request, CancellationToken cancellationToken)
			{
				return await _eventRepo.Get(request.eventParams);
			}
		}
	}
}