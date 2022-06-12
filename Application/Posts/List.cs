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
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Repositories;
using Microsoft.AspNetCore.Http;

namespace Application.Posts
{
	public class List
	{

		public class Query : IRequest<Result<List<PostDTO>>>
		{
			public PostQueryParams queryParams { get; set; }
		}

		public class Handler : IRequestHandler<Query, Result<List<PostDTO>>>
		{
			private readonly PostService _postService;
			private readonly IMapper _mapper;
			private readonly IHttpContextAccessor _httpContextAccessor;

			public Handler(PostService postService, IMapper mapper, IHttpContextAccessor httpContextAccessor)
			{
				_postService = postService;
				_mapper = mapper;
				this._httpContextAccessor = httpContextAccessor;
			}

			public async Task<Result<List<PostDTO>>> Handle(Query request, CancellationToken cancellationToken)
			{
				var res = await _postService.Get(request.queryParams);
				if (res == null) return Result<List<PostDTO>>.Failure("Posts not found!");
				_httpContextAccessor.HttpContext.Response.AddPaginationHeader<Post>(res);
				
				var PostDtos = _mapper.Map<List<PostDTO>>(res);
				return Result<List<PostDTO>>.Success(PostDtos);
			}
		}
	}
}