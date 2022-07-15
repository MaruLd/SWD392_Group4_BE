using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Domain;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Application.Locations.DTOs
{
	public class LocationQueryParams
	{
		[FromQuery(Name = "name")]
		public String? Name { get; set; }
	}
}