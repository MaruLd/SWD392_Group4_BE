using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Application.Core;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Application.Comments.DTOs
{
	public class CommentQueryParams : PaginationParams
	{
		[FromQuery(Name = "order-by")]
		public OrderByEnum OrderBy { get; set; } = OrderByEnum.DateDescending;
	}
}