using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Domain;
using Domain.Enums;

namespace Application.Events.DTOs
{
	public class EditEventDTO
	{
		[Required]
		public Guid Id { get; set; }
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