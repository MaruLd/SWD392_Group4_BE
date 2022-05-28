using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Persistence.Params
{
	public class TicketParams
	{
		[Required]
		public int EventId { get; set; }
		public string OrderBy { get; set; } = "Date";
	}
}