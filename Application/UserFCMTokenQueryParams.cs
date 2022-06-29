using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Application
{
	public class UserFCMTokenQueryParams : PaginationParams
	{
		[Required]
		[FromQuery(Name = "user-id")]
		public Guid UserId { get; set; }

		[FromQuery(Name = "order-by")]
		public OrderByEnum OrderBy { get; set; } = OrderByEnum.DateDescending;

	}
}