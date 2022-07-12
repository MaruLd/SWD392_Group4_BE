using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Events.DTOs
{
	public class EventCodeParams
	{
		public Guid EventId { get; set; }
		public EventCodeType Type { get; set; }
	}

	public enum EventCodeType
	{
		Checkin, Checkout
	}
}