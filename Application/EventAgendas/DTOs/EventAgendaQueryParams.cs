
using System.Text.Json.Serialization;
using Application.Core;
using Domain;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Application.EventAgendas.DTOs
{
	public class EventAgendaQueryParams : PaginationParams
	{
		[FromQuery(Name = "order-by")]
		public OrderByEnum OrderBy { get; set; } = OrderByEnum.DateDescending;

		[FromQuery(Name = "start-time")]
		public DateTime? StartTime { get; set; }
		[FromQuery(Name = "end-time")]
		public DateTime? EndTime { get; set; }
	}
}