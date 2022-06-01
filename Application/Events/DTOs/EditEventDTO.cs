using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Domain;

namespace Application.Events.DTOs
{
	public class EditEventDTO
	{
		[Required]
		public String? Title { get; set; }
		[Required]
		public String? Description { get; set; }

		[Required]
		public DateTime StartTime { get; set; }
		[Required]
		public DateTime EndTime { get; set; }

		[Required]
		public float Multiplier_Factor { get; set; }
		[Required]
		public int? EventCategoryId { get; set; }
		[Required]
		public EventStatus Status { get; set; }
	}
}