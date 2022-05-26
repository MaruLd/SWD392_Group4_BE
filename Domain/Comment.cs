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

		public string Title { get; set; }
		public string Content { get; set; }

		[ForeignKey("User")]
		public virtual User? User { get; set; }
		public string? UserId { get; set; }

		[ForeignKey("Post")]
		public virtual Post? Post { get; set; }
		public Guid PostId { get; set; }
	}
}