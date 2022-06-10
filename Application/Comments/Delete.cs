using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.Interfaces;
using Application.Services;
using AutoMapper;
using Domain;
using Domain.Enums;
using MediatR;
using Persistence;

namespace Application.Comments
{
	public class Delete
	{
		public class Command : IRequest<Result<Unit>>
		{
			public Guid commentId { get; set; }
		}

		public class Handler : IRequestHandler<Command, Result<Unit>>
		{
			private readonly CommentService _commentService;
			private readonly EventUserService _eventUserService;
			private readonly UserService _userService;
			private readonly IUserAccessor _userAccessor;
			private readonly IMapper _mapper;

			public Handler(EventService eventService, PostService postService, CommentService commentService, UserService userService, EventUserService eventUserService, IUserAccessor userAccessor, IMapper mapper)
			{
				this._commentService = commentService;
				this._eventUserService = eventUserService;
				this._userAccessor = userAccessor;
				this._mapper = mapper;
			}

			public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
			{
				var user = await _userService.GetByEmail(_userAccessor.GetEmail());

				var comment = await _commentService.GetByID(request.commentId);
				if (comment == null) return Result<Unit>.NotFound("Comment not found!");

				var eventUser = await _eventUserService.GetByID(comment.Post.Event.Id, user.Id);
				if (eventUser == null) return Result<Unit>.Failure("You not in the event!");

				if (!eventUser.IsModerator())
				{
					return Result<Unit>.Failure("No Permission");
				}

				comment.Status = StatusEnum.Unavailable;
				var result = await _commentService.Save();

				if (!result) return Result<Unit>.Failure("Failed to delete comment");
				return Result<Unit>.NoContentSuccess(Unit.Value); ;
			}
		}
	}
}