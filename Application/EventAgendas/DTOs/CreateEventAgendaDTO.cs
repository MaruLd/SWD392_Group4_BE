using Application.Core;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Application.EventAgendas.DTOs
{
	public class CreateEventAgendaDTO
	{
		[Required]
		public String? Title { get; set; }
		[Required]
		public String? Description { get; set; }

		[Required]
		[CheckDateRangeAttribute]
		public DateTimeOffset StartTime { get; set; }
		[Required]
		[CheckDateRangeAttribute]
		public DateTimeOffset EndTime { get; set; }
	}
}