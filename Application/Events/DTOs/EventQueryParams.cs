
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

		[SwaggerSchema("If not set, by default it won't get any DRAFT event")]
		[FromQuery(Name = "state")]
		public EventStateEnum eventStateEnum { get; set; } = EventStateEnum.None;
		[FromQuery(Name = "own-event")]
		public bool IsOwnEvent { get; set; } = false;

		[FromQuery(Name = "order-by")]
		public OrderByEnum OrderBy { get; set; } = OrderByEnum.DateDescending;

		[FromQuery(Name = "start-time")]
		public DateTime? StartTime { get; set; }
		[FromQuery(Name = "end-time")]
		public DateTime? EndTime { get; set; }

		[FromQuery(Name = "is-joined")]
		public bool IsJoined { get; set; }

	}
}