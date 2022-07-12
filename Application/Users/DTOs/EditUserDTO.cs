using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Users.DTOs
{
	public class EditUserDTO
	{
		[Required]
		public Guid Id { get; set; }
		[Required]
		public string Email { get; set; }
		[Required]
		public string DisplayName { get; set; }
		[Required]
		public string? ImageURL { get; set; }
	}
}