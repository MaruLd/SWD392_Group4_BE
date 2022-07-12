using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Domain
{
	public class EventCode
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }

		public virtual Event? Event { get; set; }
		public Guid? EventId { get; set; }

		public String Code { get; set; }

		public DateTimeOffset CreatedDate { get; set; } = DateTime.UtcNow;
		public DateTimeOffset ExpireDate { get; set; }
	}
}