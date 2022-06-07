using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Organizers.DTOs
{
	public class CreateOrganizerDTO
	{
		public String? Name { get; set; }
		public String? Description { get; set; }
		public String? ImageURL { get; set; }
	}
}