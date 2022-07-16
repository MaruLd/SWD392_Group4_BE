using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Comments.DTOs;
using Application.Core;
using Application.Services;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Comments
{
	public class List
	{

		public class Query : IRequest<Result<List<CommentDTO>>>
		{
			public Guid postId { get; set; }
			public CommentQueryParams queryParams { get; set; }
		}

		public class Handler : IRequestHandler<Query, Result<List<CommentDTO>>>
		{
			private readonly CommentService _commentService;
			private readonly IMapper _mapper;
			private readonly IHttpContextAccessor _httpContextAccessor;

			public Handler(CommentService commentService, IMapper mapper, IHttpContextAccessor httpContextAccessor)
			{
				_commentService = commentService;
				_mapper = mapper;
				_httpContextAccessor = httpContextAccessor;
			}

			public async Task<Result<List<CommentDTO>>> Handle(Query request, CancellationToken cancellationToken)
			{
				var res = await _commentService.Get(request.postId, request.queryParams);
				if (_httpContextAccessor.HttpContext.Connection == null)
					_httpContextAccessor.HttpContext.Response.AddPaginationHeader<Comment>(res);
				return Result<List<CommentDTO>>.Success(_mapper.Map<List<CommentDTO>>(res));
			}
		}
	}
}