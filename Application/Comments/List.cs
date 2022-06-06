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

			public Handler(CommentService commentService, IMapper mapper)
			{
				_commentService = commentService;
				_mapper = mapper;
			}

			public async Task<Result<List<CommentDTO>>> Handle(Query request, CancellationToken cancellationToken)
			{
				var comments = await _commentService.Get(request.postId, request.queryParams);
				return Result<List<CommentDTO>>.Success(_mapper.Map<List<CommentDTO>>(comments));
			}
		}
	}
}