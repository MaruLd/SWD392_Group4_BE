using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Domain.Enums;

namespace Domain
{
	public class TicketUser
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }

		public virtual Ticket? Ticket { get; set; }
		public Guid? TicketId { get; set; }

		public virtual User? User { get; set; }
		public Guid? UserId { get; set; }

		public DateTime? CheckedInDate { get; set; }
		public DateTime? CheckedOutDate { get; set; }
		public TicketUserStateEnum State { get; set; }
		
		public DateTime CreatedDate { get; set; } = DateTime.Now;
	}
}