using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Comments.DTOs;
using Application.Events.DTOs;
using Application.Posts.DTOs;
using Application.Tickets.DTOs;
using AutoMapper;
using Domain;

namespace Application.Core
{
	public class MappingProfiles : Profile
	{
		public MappingProfiles()
		{
			CreateMap<Event, EventDTO>()
				.ForMember(dst => dst.Tickets, src => src.MapFrom(t => t.Tickets));
			CreateMap<EventDTO, Event>();

			CreateMap<CreateEventDTO, Event>();
			CreateMap<EditEventDTO, Event>();


			CreateMap<CreateTicketDTO, Ticket>();
			CreateMap<EditTicketDTO, Ticket>();
			CreateMap<Ticket, TicketDTO>();
			
			CreateMap<CreatePostDTO, Post>();
			CreateMap<EditPostDTO, Post>();
			CreateMap<Ticket, PostDTO>();

			CreateMap<Comment, CommentDTO>();
		}

	}
}