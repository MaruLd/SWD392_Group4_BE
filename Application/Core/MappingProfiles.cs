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
			CreateMap<Event, EventDTO>()
				.ForMember(dst => dst.Tickets, opt => opt.MapFrom(et => et.EventTicket.Select(t => t.Ticket).ToList()));

			CreateMap<CreateEventDTO, Event>();
			CreateMap<Ticket, Ticket>();
		}

	}
}