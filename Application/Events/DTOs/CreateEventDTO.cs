using Application.Core;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Events.DTOs
{
	public class CreateEventDTO
	{
		[Required]
		public String? Title { get; set; }
		[Required]
		public String? Description { get; set; }
		[Required]
		public String? ImageURL { get; set; }

		[Required]
		[CheckDateRangeAttribute]
		public DateTime StartTime { get; set; }
		[Required]
		[CheckDateRangeAttribute]
		public DateTime EndTime { get; set; }

		[Required]
		public String Location { get; set; }
		[Required]
		public float MultiplierFactor { get; set; }
		[Required]
		public int? EventCategoryId { get; set; }
	}
}