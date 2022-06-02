using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Domain
{
	public class Comment
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }
		public Guid PostId { get; set; }

		public string? Body { get; set; }

		public virtual User? User { get; set; }
		public Guid UserId { get; set; }

		public virtual Post? Post { get; set; }
		

		public String Status { get; set; }
		public DateTime CreatedDate { get; set; } = DateTime.Now;
	}
}