
using System.Text.Json.Serialization;
using Application.Core;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace Application.Events.DTOs
{
	public class EventQueryParams : PaginationParams
	{
		[FromQuery(Name = "title")]
		public String? Title { get; set; }
		[FromQuery(Name = "organizer-id")]
		public Guid OrganizerId { get; set; }
		
		[FromQuery(Name = "order-by")]
		public OrderByEnum OrderBy { get; set; } = OrderByEnum.DateDescending;

		[FromQuery(Name = "start-time")]
		public DateTime? StartTime { get; set; }
		[FromQuery(Name = "end-time")]
		public DateTime? EndTime { get; set; }

	}
}