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
using Application.Users.DTOs;

namespace Application.Users
{
	public class Details
	{
		public class Query : IRequest<Result<UserDTO>>
		{
			public Guid Id { get; set; }
		}

		public class Handler : IRequestHandler<Query, Result<UserDTO>>
		{
			private readonly UserService _userService;
			private readonly IMapper _mapper;

			public Handler(UserService userService, IMapper mapper)
			{
				_userService = userService;
				_mapper = mapper;
			}


			public async Task<Result<UserDTO>> Handle(Query request, CancellationToken cancellationToken)
			{
				var user = await _userService.GetByID(request.Id);
				if (user == null) return Result<UserDTO>.NotFound("User not found!");
				return Result<UserDTO>.Success(_mapper.Map<UserDTO>(user));
			}
		}
	}
}