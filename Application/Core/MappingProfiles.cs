using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Events.DTOs;
using Application.Tickets.DTOs;
using AutoMapper;
using Domain;

namespace Application.Core
{
	public class MappingProfiles : Profile
	{
		public MappingProfiles()
		{
			CreateMap<Event, EventDTO>();
			CreateMap<Event, Event>();
			CreateMap<EventDTO, Event>();

			CreateMap<CreateEventDTO, Event>();
			CreateMap<EditEventDTO, Event>();


			CreateMap<CreateTicketDTO, Ticket>();
			CreateMap<EditTicketDTO, Ticket>();
			CreateMap<Ticket, TicketDTO>();
		}

	}
}