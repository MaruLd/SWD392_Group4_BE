using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Tickets.DTOs
{
	public class EditTicketDTO
	{
		[Required]
		public String Name { get; set; }
		[Required]
		public String Description { get; set; }
		[Required]
		public String Type { get; set; }
		[Required]
		public int Cost { get; set; }
		[Required]
		public int Quantity { get; set; }
	}
}
