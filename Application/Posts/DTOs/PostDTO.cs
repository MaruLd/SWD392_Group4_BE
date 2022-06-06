using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Enums;

namespace Application.Posts.DTOs
{
	public class PostDTO
	{
		public Guid Id { get; set; }
		public string Title { get; set; }
		public string Content { get; set; }
		public string? ImageURL { get; set; }
		public StatusEnum Status { get; set; }
		public DateTime CreatedDate { get; set; } = DateTime.Now;
	}
}
