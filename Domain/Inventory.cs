using System.ComponentModel.DataAnnotations;

namespace Domain
{
	public class Inventory
	{
		[Key]
		public int InventoryId { get; set; }
		public int Name { get; set; }

		public virtual ICollection<Ticket> Tickets { get; set; }
	}
}