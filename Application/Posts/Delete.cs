using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.Interfaces;
using Application.Posts.DTOs;
using Application.Services;
using AutoMapper;
using Domain;
using Domain.Enums;
using MediatR;
using Persistence;

namespace Application.Posts
{
	public class Delete
	{
		public class Command : IRequest<Result<Unit>>
		{
			public Guid id { get; set; }

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
			public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
			{
				var user = await _userService.GetByEmail(_userAccessor.GetEmail());
				var post = await _postService.GetByID(request.id);

				if (post == null) return Result<Unit>.Failure("Post not found!");

				var eventUser = await _eventUserService.GetByID(post.EventId.Value, user.Id);
				if (eventUser == null) return Result<Unit>.Forbidden("You aren't in the event!");
				if (!eventUser.IsModerator())
				{
					return Result<Unit>.Failure("You have no permission!");
				}

				post.Status = StatusEnum.Unavailable;

				var result = await _postService.Save();
				if (!result) return Result<Unit>.Failure("Failed to update ticket or no changes was made!");

				return Result<Unit>.Success(Unit.Value);
			}
		}
	}
}