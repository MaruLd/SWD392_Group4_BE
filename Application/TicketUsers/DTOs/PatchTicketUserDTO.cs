using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Application.TicketUsers.DTOs
{
	public class PatchTicketUserDTO
	{
		[Required]
		public Guid UserId { get; set; }
		[Required]
		public TicketUserStateEnum ticketUserStateEnum { get; set; }
	}
}