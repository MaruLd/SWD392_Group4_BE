using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.Organizers.DTOs;
using Application.Tickets.DTOs;
using Domain;

namespace Application.EventAgendas.DTOs
{
	public class EventAgendaDTO
	{
		public Guid Id { get; set; }
		public Guid EventId { get; set; }

		public String? Title { get; set; }
		public String? Description { get; set; }

		public DateTime StartTime { get; set; }
		public DateTime EndTime { get; set; }
		public DateTime CreatedDate { get; set; }
	}
}