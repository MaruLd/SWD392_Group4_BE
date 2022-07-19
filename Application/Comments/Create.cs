
using System.Data.Entity;
using Application.Comments.DTOs;
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
	public class Create
	{
		public class Command : IRequest<Result<CommentDTO>>
		{
			public Guid postid { get; set; }
			public CreateCommentDTO dto { get; set; }
		}

		public class Handler : IRequestHandler<Command, Result<CommentDTO>>
		{
			private readonly EventService _eventService;
			private readonly PostService _postService;
			private readonly CommentService _commentService;
			private readonly UserService _userService;
			private readonly EventUserService _eventUserService;
			private readonly IUserAccessor _userAccessor;
			private readonly IMapper _mapper;

			public Handler(EventService eventService, PostService postService, CommentService commentService, UserService userService, EventUserService eventUserService, IUserAccessor userAccessor, IMapper mapper)
			{
				this._eventService = eventService;
				this._postService = postService;
				this._commentService = commentService;
				this._userService = userService;
				this._eventUserService = eventUserService;
				this._userAccessor = userAccessor;
				this._mapper = mapper;
			}

			public async Task<Result<CommentDTO>>
			Handle(Command request, CancellationToken cancellationToken)
			{
				var user = await _userService.GetByID(_userAccessor.GetID());

				var post = await _postService.GetByID(request.postid);
				if (post == null) return Result<CommentDTO>.NotFound("Post not found!");

				// var eventUser = await _eventUserService.GetByID((Guid)post.EventId, user.Id);
				// if (eventUser == null) return Result<CommentDTO>.Failure("You aren't in the event!");

				var comment = _mapper.Map<Comment>(request.dto);
				comment.PostId = request.postid;
				comment.UserId = user.Id;

				var result = await _commentService.Insert(comment);

				if (!result) return Result<CommentDTO>.Failure("Failed to create comment");
				return Result<CommentDTO>.CreatedSuccess(_mapper.Map<CommentDTO>(comment));
			}
		}
	}
}
