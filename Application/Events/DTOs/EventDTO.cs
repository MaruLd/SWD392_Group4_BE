using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;

namespace Application.Events.DTOs
{
	public class EventDTO
	{
		public Guid Id { get; set; }

		public String? Title { get; set; }
		public String? Description { get; set; }

		public DateTime StartTime { get; set; }
		public DateTime EndTime { get; set; }

		public float MultiplierFactor { get; set; }

		public int? EventCategoryId { get; set; }

		public String Status { get; set; }
		public DateTime CreatedDate { get; set; } = DateTime.Now;
	}
}