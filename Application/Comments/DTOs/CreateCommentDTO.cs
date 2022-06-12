using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Comments.DTOs
{
	public class CreateCommentDTO
	{
		[Required]
		public string Body { get; set; }
	}
}
