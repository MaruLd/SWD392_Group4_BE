using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Events.DTOs
{
	public class EditEventDTO
	{
		public int Id { get; set; }
		public String? Title { get; set; }
		public String? Description { get; set; }

		public DateTime StartTime { get; set; }
		public DateTime EndTime { get; set; }

		public float Multiplier_Factor { get; set; }

		public int? EventCategoryId { get; set; }
		public String Status { get; set; }
	}
}