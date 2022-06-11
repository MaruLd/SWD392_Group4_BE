using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Microsoft.AspNetCore.Mvc;

namespace Application.TicketUsers.DTOs
{
	public class TickerUserSelfQueryParams : PaginationParams
	{
		[FromQuery(Name = "user-id")]
		public Guid UserId { get; set; }
	}
}