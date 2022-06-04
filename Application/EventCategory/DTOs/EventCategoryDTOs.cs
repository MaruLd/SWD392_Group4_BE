using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.EventCategory.DTOs
{
	public class EventCategoryDTO
	{
		public int Id { get; set; }
		public string Name { get; set; }

		[JsonIgnore]
		public int EventCount { get; set; }
	}
}