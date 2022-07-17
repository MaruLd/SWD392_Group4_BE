using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Application.Users.DTOs
{
	public class TickerUserSelfQueryParams : PaginationParams
	{
		[FromQuery(Name = "state")]
		public TicketUserStateEnum ticketUserStateEnum { get; set; } = TicketUserStateEnum.None;
	}
}