using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Enums
{
	public enum TicketUserStateEnum
	{
		None = -1,
		Idle,
		CheckedIn,
		CheckedOut,
		Ended
	}
}