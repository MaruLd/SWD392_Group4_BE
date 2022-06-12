using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.EventCategories.DTOs;
using Application.Organizers.DTOs;
using Application.Tickets.DTOs;
using Domain;
using Domain.Enums;

namespace Application.Events.DTOs
{
	public class PatchEventDTO
	{
		[Required]
		public Guid eventId { get; set; }
		[Required]
		public EventStateEnum eventStateEnum { get; set; }
	}
}