using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;

namespace Persistence.Repositories
{
	public class EventUserRepository : BaseRepository<EventUser, Guid>
	{
		public EventUserRepository(DataContext context) : base(context)
		{
		}
	}
}