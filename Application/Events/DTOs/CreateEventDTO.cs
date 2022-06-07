using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Events.DTOs
{
	public class CreateEventDTO
	{
		public String? Title { get; set; }
		public String? Description { get; set; }

		public DateTime StartTime { get; set; }
		public DateTime EndTime { get; set; }

		public float MultiplierFactor { get; set; }

		public int? EventCategoryId { get; set; }
	}
}