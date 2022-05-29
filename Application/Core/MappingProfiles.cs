using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Events.DTOs;
using AutoMapper;
using Domain;

namespace Application.Core
{
	public class MappingProfiles : Profile
	{
		public MappingProfiles()
		{
			CreateMap<Event, Event>();
			CreateMap<Event, EventDTO1>()
				.ForMember(dto => dto.Tickets, opt => opt.MapFrom(e => e.EventTicket));

			CreateMap<Ticket, Ticket>();
		}

	}
}