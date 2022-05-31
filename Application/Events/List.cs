using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.Events.DTOs;
using Application.Services;
using AutoMapper;
using AutoMapper.QueryableExtensions;
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

    public class Query : IRequest<Result<List<EventDTO>>>
    {
      public ListEventDTO dto { get; set; }
    }

    public class Handler : IRequestHandler<Query, Result<List<EventDTO>>>
    {
      private readonly DataContext _context;
      private readonly EventService _eventService;
      private readonly IMapper _mapper;

      public Handler(DataContext context, IMapper mapper, EventService eventService)
      {
        _mapper = mapper;
        _context = context;
        _eventService = eventService;
      }

      public async Task<Result<List<EventDTO>>> Handle(Query request, CancellationToken cancellationToken)
      {
        var Events = await _context.Event.ProjectTo<EventDTO>(_mapper.ConfigurationProvider).ToListAsync(cancellationToken);
        return Result<List<EventDTO>>.Success(Events);
				// return Result<List<EventDTO>>.Success(await _eventService.Get(request.dto));
        
      }
    }
  }
}