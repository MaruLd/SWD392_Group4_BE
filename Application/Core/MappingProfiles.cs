using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Comments.DTOs;
using Application.Events.DTOs;
using Application.Organizers.DTOs;
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


			CreateMap<CreateEventDTO, Event>();
			CreateMap<EditEventDTO, Event>();
			CreateMap<Event, EventDTO>()
				.ForMember(dst => dst.Tickets, src => src.MapFrom(t => t.Tickets))
				.ForMember(dst => dst.Organizers, src => src.MapFrom(o => o.Organizers));

			CreateMap<CreateTicketDTO, Ticket>();
			CreateMap<EditTicketDTO, Ticket>();
			CreateMap<Ticket, TicketDTO>();

			CreateMap<CreatePostDTO, Post>();
			CreateMap<EditPostDTO, Post>();
			CreateMap<Post, PostDTO>();

			CreateMap<Comment, CommentDTO>();

			CreateMap<CreateOrganizerDTO, Organizer>();
			CreateMap<Organizer, OrganizerDTO>();
			CreateMap<Ticket, OrganizerDTO>();

		}

	}
}