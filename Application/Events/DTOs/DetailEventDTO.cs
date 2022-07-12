using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.EventCategories.DTOs;
using Application.Organizers.DTOs;
using Application.Tickets.DTOs;
using Domain;
using Domain.Enums;

namespace Application.Events.DTOs
{
	public class DetailEventDTO
	{
		public Guid Id { get; set; }

		public String? Title { get; set; }
		public String? Description { get; set; }
		public String? ImageURL { get; set; }
		public String? Location { get; set; }

		public DateTimeOffset StartTime { get; set; }
		public DateTimeOffset EndTime { get; set; }

		public float MultiplierFactor { get; set; }

		public DateTimeOffset CreatedDate { get; set; }
		public EventStateEnum State { get; set; }

		public List<OrganizerDTO> Organizers { get; set; }
		public EventCategoryDTO EventCategory { get; set; }
	}
}