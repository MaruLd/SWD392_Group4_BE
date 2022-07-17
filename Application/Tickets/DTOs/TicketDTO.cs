using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Tickets.DTOs
{
	public class TicketDTO
	{
		public Guid Id { get; set; }
		public Guid EventID { get; set; }

		public String Name { get; set; }
		public String Description { get; set; }
		public String Type { get; set; }
		public float Cost { get; set; }
		public int Quantity { get; set; }
		public int QuantityLeft { get; set; }

		public DateTime CreatedDate { get; set; }
	}
}
