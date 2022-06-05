using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Domain;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Application.Posts.DTOs
{
	public class PostQueryParams : PaginationParams
	{
		[Required]
		[FromQuery(Name = "event-id")]
		public Guid EventId { get; set; }

		[FromQuery(Name = "title")]
		public String? Title { get; set; }

		[FromQuery(Name = "order-by")]
		public OrderByEnum OrderBy { get; set; } = OrderByEnum.DateDescending;
	}
}