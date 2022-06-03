using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace Application.Organizers.DTOs
{
	public class OrganizerQueryParams
	{
		[FromQuery(Name = "name")]
		public String? Name { get; set; }
		[FromQuery(Name = "order-by")]
		public OrderByEnum OrderBy { get; set; } = OrderByEnum.DateDescending;
	}
}