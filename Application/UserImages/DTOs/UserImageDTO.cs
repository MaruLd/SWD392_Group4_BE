using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.UserImages.DTOs
{
	public class UserImageDTO
	{
		public string Id { get; set; }
		public string Url { get; set; }
		public DateTime CreatedDate { get; set; }
	}
}