using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.Events.DTOs;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Application.Users.DTOs
{
	public class EventSelfQueryParams : EventQueryParams
	{
		[FromQuery(Name = "is-own")]
		public bool IsOwn { get; set; } = false;
	}
}