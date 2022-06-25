using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Domain.Enums;

namespace Application.EventUsers.DTOs
{
	public class EditEventUserDTO
	{
		[Required]
		public Guid UserId { get; set; }
		[Required]
		public EventUserTypeEnum Type { get; set; }
	}
}