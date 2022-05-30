using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Tickets.DTOs
{
	public class TicketDTO1
	{

		public Guid Id { get; set; }

		public String Name { get; set; }
		public String Description { get; set; }
		public String Type { get; set; }
		public int Cost { get; set; }
		public int Quantity { get; set; }

		public DateTime CreatedDate { get; set; }
	}
}
