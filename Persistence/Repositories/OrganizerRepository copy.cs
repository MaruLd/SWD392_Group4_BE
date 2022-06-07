using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;

namespace Persistence.Repositories
{
	public class EventOrganizerRepository : BaseRepository<EventOrganizer, Guid>
	{
		public EventOrganizerRepository(DataContext context) : base(context)
		{
		}
	}
}