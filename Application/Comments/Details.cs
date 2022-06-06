using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Comments.DTOs;
using Application.Core;
using Application.Interfaces;
using Application.Services;
using AutoMapper;
using Domain;
using MediatR;
using Persistence;

namespace Application.Comments
{
	public class Details
	{
		public class Query : IRequest<Result<CommentDTO>>
		{
			public Guid postId { get; set; }
		}

		public class Handler : IRequestHandler<Query, Result<CommentDTO>>
		{
			private readonly CommentService _commentService;
			private readonly UserService _userService;
			private readonly IUserAccessor _userAccessor;
			private readonly IMapper _mapper;

			public Handler(EventService eventService, PostService postService, CommentService commentService, UserService userService, EventUserService eventUserService, IUserAccessor userAccessor, IMapper mapper)
			{
				this._commentService = commentService;
				this._userAccessor = userAccessor;
				this._mapper = mapper;
			}

			public async Task<Result<CommentDTO>> Handle(Query request, CancellationToken cancellationToken)
			{
				var comment = await _commentService.GetByID(request.postId);
				if (comment == null) return Result<CommentDTO>.NotFound("Comment not found!");
				return Result<CommentDTO>.NoContentSuccess(_mapper.Map<CommentDTO>(comment));
			}
		}
	}
}