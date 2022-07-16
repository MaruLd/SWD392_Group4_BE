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
using Domain.Enums;

namespace Application.Posts
{
	public class Create
	{
		public class Command : IRequest<Result<PostDTO>>
		{
			public CreatePostDTO dto { get; set; }
		}

		public class Handler : IRequestHandler<Command, Result<PostDTO>>
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

			public async Task<Result<PostDTO>>
			Handle(Command request, CancellationToken cancellationToken)
			{
				var user = await _userService.GetByID(_userAccessor.GetID());
				var eventUser = await _eventUserService.GetByID(request.dto.EventID, user.Id);

				if (eventUser == null) return Result<PostDTO>.Failure("You aren't in the event!");
				if (!eventUser.IsModerator())
				{
					return Result<PostDTO>.Failure("You have no permission!");
				}

				var post = _mapper.Map<Post>(request.dto);
				post.UserId = user.Id;

				var result = await _postService.Insert(post);

				if (!result) return Result<PostDTO>.Failure("Failed to create Post");

				return Result<PostDTO>.CreatedSuccess(_mapper.Map<PostDTO>(post));
			}
		}
	}
}
