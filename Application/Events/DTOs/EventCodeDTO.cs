using System;
using System.Collections.Generic;
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
	public class EventCodeDTO
	{
		public String Code { get; set; }
		public DateTime CreatedDate { get; set; }
		public DateTime ExpireDate { get; set; }
	}
}