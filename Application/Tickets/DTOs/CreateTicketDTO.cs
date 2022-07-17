using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Tickets.DTOs
{
	public class CreateTicketDTO
	{
		[Required]
		public Guid EventId { get; set; }
		[Required]
		public String Name { get; set; }
		[Required]
		public String Description { get; set; }
		[Required]
		public String Type { get; set; }

		[Required]
		[Range(0, 100)]
		public float Cost { get; set; }
		[Required]
		[Range(0, 10000)]
		public int Quantity { get; set; }
	}
}