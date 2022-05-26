using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Domain
{
	public class EventAgenda
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }

		public String? Title { get; set; }
		public String? Description { get; set; }

		public DateTime StartTime { get; set; }
		public DateTime EndTime { get; set; }

		public virtual Event? Event { get; set; }
		public int? EventId { get; set; }

		public DateTime CreatedDate { get; set; } = DateTime.Now;
	}
}