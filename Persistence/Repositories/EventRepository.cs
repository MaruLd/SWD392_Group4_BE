using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{
	public class EventRepository : BaseRepository<Event, Guid>
	{
		public EventRepository(DataContext context) : base(context)
		{
		}
	}
}