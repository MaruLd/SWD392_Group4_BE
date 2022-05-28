using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Persistence.Params
{
	public class EventParams
	{
		public String? Title { get; set; }
		public string OrderBy { get; set; } = "Date";

		public DateTime? StartTime { get; set; }
		public DateTime? EndTime { get; set; }

	}
}