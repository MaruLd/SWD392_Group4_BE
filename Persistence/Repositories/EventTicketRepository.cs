using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;

namespace Persistence.Repositories
{
	public class EventTicketRepository : BaseRepository<EventTicket, Guid>
	{
		public EventTicketRepository(DataContext context) : base(context)
		{
		}
	}
}