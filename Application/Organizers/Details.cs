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
using Application.Organizers.DTOs;

namespace Application.Organizers
{
	public class Details
	{
		public class Query : IRequest<Result<OrganizerDTO>>
		{
			public Guid Id { get; set; }
		}

		public class Handler : IRequestHandler<Query, Result<OrganizerDTO>>
		{
			private readonly OrganizerService _organizerService;
			private readonly IMapper _mapper;

			public Handler(OrganizerService organizerService, IMapper mapper)
			{
				_organizerService = organizerService;
				_mapper = mapper;
			}


			public async Task<Result<OrganizerDTO>> Handle(Query request, CancellationToken cancellationToken)
			{
				var o = _organizerService.GetByID(request.Id);
				if (o == null) return Result<OrganizerDTO>.Failure("Organizer not found!");
				var dto = _mapper.Map<OrganizerDTO>(o);
				return Result<OrganizerDTO>.Success(dto);
			}
		}
	}
}