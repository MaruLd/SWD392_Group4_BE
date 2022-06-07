using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Organizers.DTOs;
using Application.Tickets.DTOs;
using Application.Users.DTOs;
using Domain;
using Domain.Enums;

namespace Application.EventUsers.DTOs
{
	public class EventUserDTO
	{
		public EventUserTypeEnum Type { get; set; }
		public UserDTO User { get; set; }

		public DateTime CreatedDate { get; set; }
	}
}