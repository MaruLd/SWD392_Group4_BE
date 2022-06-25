using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Organizers.DTOs;
using Application.Tickets.DTOs;
using Application.Users.DTOs;
using Domain;
using Domain.Enums;

namespace Application.TicketUsers.DTOs
{
	public class TicketUserDTO
	{
		// public TicketDTO Ticket { get; set; }
		public UserDTO? User { get; set; }
		public DateTime CreatedDate { get; set; }

		public DateTime? CheckedInDate { get; set; }
		public DateTime? CheckedOutDate { get; set; }
		public TicketUserStateEnum State { get; set; }
	}
}