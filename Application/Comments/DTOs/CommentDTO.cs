using Application.Users.DTOs;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Comments.DTOs
{
	public class CommentDTO
	{
		public Guid PostId { get; set; }
		public Guid Id { get; set; }

		public string Body { get; set; }
		
		public UserDTO User { get; set; }

		public DateTime CreatedDate { get; set; }
	}
}
