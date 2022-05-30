using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.Interfaces;
using AutoMapper;
using Domain;
using FluentValidation;
using MediatR;
using Persistence;

namespace Application.Events
{
  public class Create
  {
    public class Command : IRequest<Result<Unit>> //Command do not return anything, but can return success or failure, return Unit also meant for nothing
    {
      public Event Event { get; set; }

    }

    public class CommandValidator : AbstractValidator<Command>
    {
      public CommandValidator()
      {
        RuleFor(x => x.Event).SetValidator(new EventValidator());
      }

    }

    public class Handler : IRequestHandler<Command, Result<Unit>>
    {
      private readonly DataContext _context;
      private readonly IMapper _mapper;
      private readonly IUserAccessor _userAccessor;

      public Handler(DataContext context, IMapper mapper, IUserAccessor userAccessor)
      {
        _userAccessor = userAccessor;
        _context = context;
        _mapper = mapper;
      }

      public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
      {
				var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername());

				var eventUser = new EventUser{
          Status = "Not Attended",
					User = user,
					Event = request.Event,
					Type = EventUserType.Admin
				};

        request.Event.EventUser.Add(eventUser);

        Event e = _mapper.Map<Event>(request.Event);
        _context.Event.Add(e);

        var result = await _context.SaveChangesAsync() > 0; //if nothing written to the DB then this will return 0

        if (!result) return Result<Unit>.Failure("Failed to create event");

        return Result<Unit>.Success(Unit.Value); //Unit.Value is nothing
      }
    }
  }
}