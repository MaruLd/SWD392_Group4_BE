using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Events.DTOs;
using Application.Organizers.DTOs;
using Application.Tickets.DTOs;
using Application.Users.DTOs;
using Domain;
using Domain.Enums;

namespace Application.EventOrganizers.DTOs
{
	public class EventOrganizerDTO
	{
		public OrganizerDTO Organizer { get; set; }
	}
}