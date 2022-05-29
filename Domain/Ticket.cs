using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Domain
{
	public class Ticket
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }

		public String Type { get; set; }
		public int Cost { get; set; }
		public int Quantity { get; set; }

		public virtual ICollection<User> Users { get; set; }
		public DateTime CreatedDate { get; set; } = DateTime.Now;

		public ICollection<EventTicket> EventTicket { get; set; }
	}
}