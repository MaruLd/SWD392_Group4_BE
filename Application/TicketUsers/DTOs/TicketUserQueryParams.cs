
using System.Text.Json.Serialization;
using Application.Core;
using Domain;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Application.TicketUsers.DTOs
{
	public class TicketUserQueryParams : PaginationParams
	{
		[FromQuery(Name = "name")]
		public String? DisplayName { get; set; }
		[FromQuery(Name = "email")]
		public String? Email { get; set; }

	}
}