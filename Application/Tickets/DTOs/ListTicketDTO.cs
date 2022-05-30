using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Tickets.DTOs
{
	public class ListTicketDTO
	{
		[Required]
		public Guid EventId { get; set; }
		public string? OrderBy { get; set; } = "Date";
	}
}