using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Users.DTOs;

namespace Application.Tickets.DTOs
{
	public class DetailTicketDTO
	{
		public Guid Id { get; set; }
		public Guid EventID { get; set; }

		public String Name { get; set; }
		public String Description { get; set; }
		public String Type { get; set; }
		public int Cost { get; set; }
		public int Quantity { get; set; }
		public int QuantityLeft { get; set; }

		public List<UserDTO> Users { get; set; }
		public DateTime CreatedDate { get; set; }
	}
}
