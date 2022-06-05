using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Posts.DTOs
{
	public class CreatePostDTO
	{
		public Guid EventID { get; set; }
		public string Title { get; set; }
		public string Content { get; set; }
	}
}