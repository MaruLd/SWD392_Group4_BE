using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Application.EventUsers.DTOs
{
	public class CreateEventUserDTO
	{
		[Required]
		public Guid UserId { get; set; }
		[Required]
		public EventUserTypeEnum Type { get; set; }
	}
}