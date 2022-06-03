using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{
	public class TicketRepository : BaseRepository<Ticket, Guid>
	{
		public TicketRepository(DataContext context) : base(context)
		{
		}
	}
}