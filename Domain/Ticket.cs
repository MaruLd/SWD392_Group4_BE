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
	public class Ticket
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }

		public String Name { get; set; }
		public String Description { get; set; }
		public String Type { get; set; }
		public float Cost { get; set; }
		public int Quantity { get; set; }

		public DateTime CreatedDate { get; set; } = DateTime.Now;
		
		public StatusEnum Status { get; set; }
		[JsonIgnore]
		public virtual Event? Event { get; set; }
		public Guid? EventId { get; set; }

		[JsonIgnore]
		public virtual ICollection<TicketUser> TicketUsers { get; set; }
		// [JsonIgnore]
		// public virtual ICollection<EventTicket> EventTicket { get; set; }



	}
}