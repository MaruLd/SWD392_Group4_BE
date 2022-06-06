using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Application.Core;
using Application.Events.DTOs;
using Application.Interfaces;
using Application.Organizers.DTOs;
using Application.Services;
using AutoMapper;
using Domain;
using FluentValidation;
using MediatR;
using Persistence;

namespace Application.Organizers
{
	public class Create
	{
		public class Command : IRequest<Result<OrganizerDTO>> //Command do not return anything, but can return success or failure, return Unit also meant for nothing
		{
			public CreateOrganizerDTO dto { get; set; }

		}

		// public class CommandValidator : AbstractValidator<Command>
		// {
		// 	public CommandValidator()
		// 	{
		// 		RuleFor(x => x.Event).SetValidator(new EventValidator());
		// 	}

		// }

		public class Handler : IRequestHandler<Command, Result<OrganizerDTO>>
		{
			private readonly OrganizerService _organizerService;
			private readonly IMapper _mapper;

			public Handler(OrganizerService organizerService, IMapper mapper)
			{
				_mapper = mapper;
				_organizerService = organizerService;

			}

			public async Task<Result<OrganizerDTO>> Handle(Command request, CancellationToken cancellationToken)
			{
				var o = _mapper.Map<Organizer>(request.dto);
				var result = await _organizerService.Insert(o);

				if (!result) return Result<OrganizerDTO>.Failure("Failed to create organizer");
				return Result<OrganizerDTO>.CreatedSuccess(_mapper.Map<OrganizerDTO>(o));
			}
		}
	}
}