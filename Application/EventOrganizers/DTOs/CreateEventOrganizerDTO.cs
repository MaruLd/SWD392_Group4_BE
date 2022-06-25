using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Application.EventOrganizers.DTOs
{
	public class CreateEventOrganizerDTO
	{
		[Required]
		public Guid OrganizerId { get; set; }
	}
}