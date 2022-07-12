using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.Events.DTOs;
using Application.Services;
using AutoMapper;
using Domain;
using MediatR;
using Persistence;

namespace Application.Events
{
	public class GetCode
	{
		public class Query : IRequest<Result<EventCodeDTO>>
		{
			public EventCodeParams dto { get; set; }
		}

		public class Handler : IRequestHandler<Query, Result<EventCodeDTO>>
		{
			private readonly EventService _eventService;
			private readonly EventCodeService _eventCodeService;
			private readonly IMapper _mapper;

			public Handler(EventService eventService, EventCodeService eventCodeService, IMapper mapper)
			{
				;
				_eventService = eventService;
				this._eventCodeService = eventCodeService;
				_mapper = mapper;
			}

			public async Task<Result<EventCodeDTO>> Handle(Query request, CancellationToken cancellationToken)
			{
				var e = await _eventService.GetByID(request.dto.EventId);
				if (e == null) return Result<EventCodeDTO>.Failure("Event not found!");

				var ec = await _eventCodeService.GetByID(request.dto.EventId);
				if (ec == null)
				{
					ec = new EventCode();
					ec.EventId = e.Id;
					ec.Code = RandomUtil.GenerateRandomCode();
					ec.ExpireDate = DateTime.Now.AddMinutes(5);
				}

				if (ec.ExpireDate >= DateTime.Now)
				{
					ec.Code = RandomUtil.GenerateRandomCode();
					ec.ExpireDate = DateTime.Now.AddMinutes(5);
					await _eventCodeService.Update(ec);
				}

				var dto = _mapper.Map<EventCodeDTO>(e);
				return Result<EventCodeDTO>.Success(dto);
			}
		}
	}
}