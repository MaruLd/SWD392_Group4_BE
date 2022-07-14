using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Users.DTOs
{
	public class UserDTO
	{
		public Guid Id { get; set; }
		public string Email { get; set; }
		public string DisplayName { get; set; }
		public int Bean { get; set; }

		public string? ImageURL { get; set; }
	}
}