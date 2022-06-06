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
using Application.Users.DTOs;

namespace Application.Users
{
	public class List
	{

		public class Query : IRequest<Result<List<UserDTO>>>
		{
			public UserQueryParams queryParams { get; set; }
		}

		public class Handler : IRequestHandler<Query, Result<List<UserDTO>>>
		{
			private readonly UserService _userService;
			private readonly IMapper _mapper;

			public Handler(UserService userService, IMapper mapper)
			{
				_userService = userService;
				_mapper = mapper;
			}

			public async Task<Result<List<UserDTO>>> Handle(Query request, CancellationToken cancellationToken)
			{
				var res = await _userService.Get(request.queryParams);
				return Result<List<UserDTO>>.Success(_mapper.Map<List<UserDTO>>(res));
			}
		}
	}
}