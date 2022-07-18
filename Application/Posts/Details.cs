using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.Services;
using Application.Posts.DTOs;
using AutoMapper;
using Domain;
using MediatR;
using Persistence;

namespace Application.Posts
{
	public class Details
	{
		public class Query : IRequest<Result<PostDTO>>
		{
			public Guid Id { get; set; }
		}

		public class Handler : IRequestHandler<Query, Result<PostDTO>>
		{
			private readonly PostService _postService;
			private readonly IMapper _mapper;

			public Handler(PostService postService, IMapper mapper)
			{
				_postService = postService;
				_mapper = mapper;
			}


			public async Task<Result<PostDTO>> Handle(Query request, CancellationToken cancellationToken)
			{
				var Post = await _postService.GetByID(request.Id);
				var PostDto = _mapper.Map<PostDTO>(Post);
				return Result<PostDTO>.Success(PostDto);
			}
		}
	}
}