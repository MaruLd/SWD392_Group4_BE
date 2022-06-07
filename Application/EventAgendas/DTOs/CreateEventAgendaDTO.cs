using Application.Core;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.EventAgendas.DTOs
{
	public class CreateEventAgendaDTO
	{
		public String? Title { get; set; }
		public String? Description { get; set; }

		[CheckDateRangeAttribute]
		public DateTime StartTime { get; set; }
		[CheckDateRangeAttribute]
		public DateTime EndTime { get; set; }
	}
}