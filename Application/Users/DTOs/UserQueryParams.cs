using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Application.Users.DTOs
{
	public class UserQueryParams : PaginationParams
	{
		[FromQuery(Name = "email")]
		public string Email { get; set; }
		[FromQuery(Name = "display-name")]
		public string DisplayName { get; set; }

		[FromQuery(Name = "order-by")]
		public OrderByEnum OrderBy { get; set; } = OrderByEnum.DateDescending;
	}
}