using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Posts.DTOs
{
	public class PostDTO
	{
		public Guid Id { get; set; }
		public Guid EventID { get; set; }
		public Guid? UserId { get; set; }
		public String Title { get; set; }
		public String Description { get; set; }
		public String Type { get; set; }
		public int Cost { get; set; }
		public int Quantity { get; set; }

		public DateTime CreatedDate { get; set; }
	}
}
