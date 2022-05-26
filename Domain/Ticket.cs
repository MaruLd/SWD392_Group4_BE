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

		[ForeignKey("Event")]
		public virtual Event? Event { get; set; }
		public String? EventId { get; set; }

		public virtual ICollection<User> Users { get; set; }
	}
}