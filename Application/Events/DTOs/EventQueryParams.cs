
using System.Text.Json.Serialization;
using Application.Core;
using Domain;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Application.Events.DTOs
{
	public class EventQueryParams : PaginationParams
	{
		[FromQuery(Name = "title")]
		public String? Title { get; set; }
		[FromQuery(Name = "organizer-name")]
		public String? OrganizerName { get; set; }
		[FromQuery(Name = "category-id")]
		public int CategoryId { get; set; }
		[FromQuery(Name = "location")]
		public String? Location { get; set; }

		[FromQuery(Name = "state")]
		public EventStateEnum eventStateEnum { get; set; } = EventStateEnum.None;

		[FromQuery(Name = "order-by")]
		public OrderByEnum OrderBy { get; set; } = OrderByEnum.DateDescending;

		[FromQuery(Name = "start-time")]
		public DateTime? StartTime { get; set; }
		[FromQuery(Name = "end-time")]
		public DateTime? EndTime { get; set; }
	}
}