using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.EventCategories.DTOs;
using Application.Organizers.DTOs;
using Application.Tickets.DTOs;
using Application.Users.DTOs;
using Domain;
using Domain.Enums;

namespace Application.Users.DTOs
{
	public class SelfEventDTO
	{
		public Guid Id { get; set; }

		public String? Title { get; set; }
		public String? ImageURL { get; set; }
		public String? Location { get; set; }

		public DateTime StartTime { get; set; }
		public DateTime EndTime { get; set; }

		public float MultiplierFactor { get; set; }

		public DateTime CreatedDate { get; set; }
		public EventStateEnum State { get; set; }
		public EventUserTypeEnum UserType { get; set; }

		public List<OrganizerDTO> Organizers { get; set; }
		public EventCategoryDTO EventCategory { get; set; }
	}
}