using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Users.DTOs;
using Domain.Enums;

namespace Application.Posts.DTOs
{
	public class PostDTO
	{
		public Guid Id { get; set; }
		public string Title { get; set; }
		public string Content { get; set; }
		public string? ImageURL { get; set; }
		public DateTime CreatedDate { get; set; }
		public UserDTO User { get; set; }
	}
}
