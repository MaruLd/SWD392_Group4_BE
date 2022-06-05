using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Comments.DTOs
{
	public class EditCommentDTO
	{
		[Required]
		public Guid PostId { get; set; }
		[Required]
		public Guid CommentId { get; set; }
		[Required]
		public string Body { get; set; }
	}
}
