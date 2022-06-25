using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Organizers.DTOs;
using Application.Tickets.DTOs;
using Application.Users.DTOs;
using Domain;
using Domain.Enums;

namespace Application.Users.DTOs
{
	public class SelfTicketDTO
	{
		public Guid Id { get; set; }

		public Guid EventId { get; set; }
		public Guid TicketId { get; set; }

		public String Name { get; set; }
		public String Description { get; set; }
		public String Type { get; set; }

		public DateTime? CheckedInDate { get; set; }
		public DateTime? CheckedOutDate { get; set; }
		public TicketUserStateEnum State { get; set; }

		public DateTime CreatedDate { get; set; }
	}
}