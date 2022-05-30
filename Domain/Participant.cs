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
		public Guid Id { get; set; }
		public String Status { get; set; }
		public ParticipantType Type { get; set; }

		public DateTime CreatedDate { get; set; } = DateTime.Now;
		
		public virtual Event? Event { get; set; }
		public Guid? EventId { get; set; }

		public virtual User? User { get; set; }
		public Guid? UserId { get; set; }
	}
}