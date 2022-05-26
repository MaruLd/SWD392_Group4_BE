using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Domain
{
	public class Participant
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		public String Status { get; set; }
		public ParticipantType Type { get; set; }

		public virtual Event? Event { get; set; }
		public int? EventId { get; set; }

		public virtual User? User { get; set; }
		public int? UserId { get; set; }

		public DateTime CreatedDate { get; set; } = DateTime.Now;
	}
}