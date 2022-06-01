
using System.Text.Json.Serialization;
using Domain;

namespace Application.Events.DTOs
{
	public class ListEventParams
	{
		public String? Title { get; set; }
		public OrderByEnum OrderBy { get; set; } = OrderByEnum.DateAscending;

		public DateTime? StartTime { get; set; }
		public DateTime? EndTime { get; set; }

	}
}