using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
	public class EventTicket
	{
		public Guid TicketId { get; set; }
		public virtual Ticket Ticket { get; set; }

		public Guid EventId { get; set; }
		public virtual Event Event { get; set; }

		public bool IsValid { get; set; }
	}
}