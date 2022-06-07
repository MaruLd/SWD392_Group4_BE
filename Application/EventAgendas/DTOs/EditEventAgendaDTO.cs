using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Domain;
using Domain.Enums;

namespace Application.EventAgendas.DTOs
{
	public class EditEventAgendaDTO
	{
		public Guid Id { get; set; }

		public String? Title { get; set; }
		public String? Description { get; set; }
		
		[CheckDateRangeAttribute]
		public DateTime StartTime { get; set; }
		[CheckDateRangeAttribute]
		public DateTime EndTime { get; set; }
	}
}