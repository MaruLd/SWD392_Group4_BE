using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Domain
{
	public class CommentLike
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }

		public virtual Comment? Comment { get; set; }
		public Guid CommentId { get; set; }

		public virtual User? User { get; set; }
		public Guid UserId { get; set; }
		
		public StatusEnum Status { get; set; }
		public DateTimeOffset CreatedDate { get; set; } = DateTime.UtcNow;
	}
}