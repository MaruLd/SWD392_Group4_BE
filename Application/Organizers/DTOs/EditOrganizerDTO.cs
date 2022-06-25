using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Domain;

namespace Application.Events.DTOs
{
	public class EditOrganizerDTO
	{
		[Required]
		public Guid Id { get; set; }

		[Required]
		public String? Name { get; set; }
		[Required]
		public String? Description { get; set; }
		[Required]
		public String? ImageURL { get; set; }
	}
}