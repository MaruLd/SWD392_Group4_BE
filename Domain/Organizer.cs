using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Domain
{
	public class Organizer
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		public String? Name { get; set; }
		public String? Description { get; set; }

		public String Status { get; set; }
		public DateTime CreatedDate { get; set; } = DateTime.Now;

		public virtual ICollection<Event> Events { get; set; }
	}
}