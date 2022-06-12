using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Comments.DTOs;
using Application.EventAgendas.DTOs;
using Application.EventCategories.DTOs;
using Application.EventOrganizers.DTOs;
using Application.Events.DTOs;
using Application.EventUsers.DTOs;
using Application.Organizers.DTOs;
using Application.Posts.DTOs;
using Application.Tickets.DTOs;
using Application.TicketUsers.DTOs;
using Application.Users.DTOs;
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
			CreateMap<EventDTO, Event>();
			CreateMap<Event, EventDTO>()
				.ForMember(dst => dst.Organizers, src => src.MapFrom(o => o.EventOrganizers.Select(eo => eo.Organizer).ToList()))
				.ForMember(dst => dst.EventCategory, src => src.MapFrom(o => o.EventCategory));

			CreateMap<Event, DetailEventDTO>()
				.ForMember(dst => dst.Tickets, src => src.MapFrom(t => t.Tickets))
				.ForMember(dst => dst.Organizers, src => src.MapFrom(o => o.EventOrganizers.Select(eo => eo.Organizer).ToList()))
				.ForMember(dst => dst.EventCategory, src => src.MapFrom(o => o.EventCategory));

			CreateMap<CreateTicketDTO, Ticket>();
			CreateMap<EditTicketDTO, Ticket>();
			CreateMap<TicketDTO, Ticket>();
			CreateMap<Ticket, TicketDTO>()
				.ForMember(t => t.QuantityLeft, opt => opt.MapFrom(t => t.Quantity - t.TicketUsers.Select(tu => tu.User).Count()));

			CreateMap<CreatePostDTO, Post>();
			CreateMap<EditPostDTO, Post>();
			CreateMap<Post, PostDTO>();
			CreateMap<PostDTO, Post>();

			CreateMap<CreateCommentDTO, Comment>();
			CreateMap<Comment, CommentDTO>();
			CreateMap<CommentDTO, Comment>();

			CreateMap<CreateOrganizerDTO, Organizer>();
			CreateMap<EditOrganizerDTO, Organizer>();
			CreateMap<OrganizerDTO, Organizer>();
			CreateMap<Organizer, OrganizerDTO>();

			CreateMap<EventCategory, EventCategoryDTO>();
			CreateMap<EventCategoryDTO, EventCategory>();

			CreateMap<CreateEventAgendaDTO, EventAgenda>();
			CreateMap<EditEventAgendaDTO, EventAgenda>();
			CreateMap<EventAgenda, EventAgendaDTO>();
			CreateMap<EventAgendaDTO, EventAgenda>();

			CreateMap<EditUserDTO, User>();
			CreateMap<User, UserDTO>();
			CreateMap<UserDTO, User>();

			CreateMap<TicketUser, SelfTicketDTO>()
				.ForMember(st => st.Name, src => src.MapFrom(t => t.Ticket.Name))
				.ForMember(st => st.Description, src => src.MapFrom(t => t.Ticket.Description))
				.ForMember(st => st.Type, src => src.MapFrom(t => t.Ticket.Type))

				.ForMember(st => st.EventId, src => src.MapFrom(t => t.Ticket.EventId))
				.ForMember(st => st.TicketId, src => src.MapFrom(t => t.Ticket.Id));


			CreateMap<CreateEventUserDTO, EventUser>();
			CreateMap<EditEventUserDTO, EventUser>();
			CreateMap<EventUser, EventUserDTO>();
			CreateMap<EventUserDTO, EventUser>();

			CreateMap<CreateTicketUserDTO, TicketUser>();
			// CreateMap<EditTicketUserDTO, TicketUser>();
			CreateMap<TicketUser, TicketUserDTO>();
			CreateMap<TicketUserDTO, TicketUser>();

			CreateMap<CreateEventOrganizerDTO, EventOrganizer>();
			// CreateMap<EditTicketUserDTO, TicketUser>();
			CreateMap<EventOrganizer, EventOrganizerDTO>();
			CreateMap<EventOrganizerDTO, EventOrganizer>();
		}

	}
}