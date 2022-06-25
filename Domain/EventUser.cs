using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Domain.Enums;

namespace Domain
{
	public class EventUser
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }

		public StatusEnum Status { get; set; }
		public EventUserTypeEnum Type { get; set; }

		public virtual Event? Event { get; set; }
		public Guid? EventId { get; set; }

		public virtual User? User { get; set; }
		public Guid? UserId { get; set; }


		public DateTime CreatedDate { get; set; } = DateTime.Now;
	}
}