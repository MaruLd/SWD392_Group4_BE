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
		public int Id { get; set; }

		public string Title { get; set; }
		public string Content { get; set; }

		public virtual User? User { get; set; }
		public int? UserId { get; set; }

		public virtual Post? Post { get; set; }
		public int PostId { get; set; }
	}
}