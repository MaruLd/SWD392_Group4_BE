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
using Application.Organizers.DTOs;

namespace Application.Organizers
{
	public class List
	{

		public class Query : IRequest<Result<List<OrganizerDTO>>>
		{
			public OrganizerQueryParams queryParams { get; set; }
		}

		public class Handler : IRequestHandler<Query, Result<List<OrganizerDTO>>>
		{
			private readonly OrganizerService _organizerService;
			private readonly IMapper _mapper;

			public Handler(OrganizerService organizerService, IMapper mapper)
			{
				_organizerService = organizerService;
				_mapper = mapper;
			}

			public async Task<Result<List<OrganizerDTO>>> Handle(Query request, CancellationToken cancellationToken)
			{
				var res = await _organizerService.Get(request.queryParams);
				if (res == null) return Result<List<OrganizerDTO>>.Failure("Organizers not found!");
				var dto = _mapper.Map<List<OrganizerDTO>>(res);
				return Result<List<OrganizerDTO>>.Success(dto);
			}
		}
	}
}