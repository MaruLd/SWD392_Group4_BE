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
using Application.Users.DTOs;

namespace Application.Users
{
	public class Edit
	{
		public class Command : IRequest<Result<Unit>>
		{
			public EditUserDTO dto { get; set; }

		}

		public class Handler : IRequestHandler<Command, Result<Unit>>
		{
			private readonly UserService _userService;
			private readonly IUserAccessor _userAccessor;
			private readonly IMapper _mapper;

			public Handler(UserService userService, IUserAccessor userAccessor, IMapper mapper)
			{
				this._userService = userService;
				this._userAccessor = userAccessor;
				this._mapper = mapper;
			}
			public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
			{
				var userLogged = await _userService.GetByEmail(_userAccessor.GetEmail());
				var userInDb = await _userService.GetByID(request.dto.Id);
				if (userInDb == null) return Result<Unit>.Failure("User not found!");

				if (userLogged.Id != userInDb.Id) return Result<Unit>.Forbidden("You can't change other's profile!");

				_mapper.Map<EditUserDTO, User>(request.dto, userInDb);

				var result = await _userService.Save();
				if (!result) return Result<Unit>.Failure("Failed to update profile or it's the same!");
				return Result<Unit>.NoContentSuccess(Unit.Value);
			}
		}
	}
}