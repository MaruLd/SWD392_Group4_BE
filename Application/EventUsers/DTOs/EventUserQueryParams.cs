
using System.Text.Json.Serialization;
using Application.Core;
using Domain;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Application.EventUsers.DTOs
{
	public class EventUserQueryParams : PaginationParams
	{
		[FromQuery(Name = "status")]
		public EventUserStatusEnum Status { get; set; }
		[FromQuery(Name = "type")]
		public EventUserTypeEnum Type { get; set; }

		[FromQuery(Name = "name")]
		public String? DisplayName { get; set; }
		[FromQuery(Name = "email")]
		public String? Email { get; set; }

	}
}