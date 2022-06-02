using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.Interfaces;
using Application.Services;
using Application.Posts.DTOs;
using AutoMapper;
using Domain;
using FluentValidation;
using MediatR;
using Persistence;

namespace Application.Posts
{
  public class Create
  {
    public class Command : IRequest<Result<Unit>>
    {
      public CreatePostDTO dto { get; set; }
    }

    public class Handler : IRequestHandler<Command, Result<Unit>>
    {
      private readonly EventService _eventService;
      private readonly PostService _postService;
      private readonly UserService _userService;
      private readonly EventUserService _eventUserService;
      private readonly IUserAccessor _userAccessor;
      private readonly IMapper _mapper;

      public Handler(EventService eventService, PostService postService, UserService userService, EventUserService eventUserService, IUserAccessor userAccessor, IMapper mapper)
      {
        this._eventService = eventService;
        this._postService = postService;
        this._userService = userService;
        this._eventUserService = eventUserService;
        this._userAccessor = userAccessor;
        this._mapper = mapper;
      }

      public async Task<Result<Unit>>
      Handle(Command request, CancellationToken cancellationToken)
      {
        var user = await _userService.GetByEmail(_userAccessor.GetEmail());
        var eventUser = await _eventUserService.GetByID(request.dto.EventID, user.Id);

        if (eventUser == null) return Result<Unit>.Failure("You aren't in the event!");

        var allowedRole = new List<EventUserType> { EventUserType.Admin, EventUserType.Manager };
        if (!allowedRole.Contains(eventUser.Type))
        {
          return Result<Unit>.Failure("You have no permission!");
        }

        var Post = _mapper.Map<Post>(request.dto);

        var result = await _postService.Insert(Post);
        if (!result) return Result<Unit>.Failure("Failed to create Post");

        return Result<Unit>.Success(Unit.Value);
      }
    }
  }
}
