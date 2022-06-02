using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Posts.DTOs
{
	public class EditPostDTO
	{
		[Required]
		public String Title { get; set; }
		[Required]
		public String Description { get; set; }
		[Required]
		public String Type { get; set; }
		[Required]
		public int Cost { get; set; }
		[Required]
		public int Quantity { get; set; }
	}
}
