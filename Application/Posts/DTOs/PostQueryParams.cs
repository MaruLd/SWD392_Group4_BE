using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Application.Posts.DTOs
{
	public class PostQueryParams
	{
		[Required]
		[FromQuery(Name = "event-id")]
		public Guid EventId { get; set; }

		[FromQuery(Name = "order-by")]
		public OrderByEnum OrderBy { get; set; } = OrderByEnum.DateDescending;
	}
}