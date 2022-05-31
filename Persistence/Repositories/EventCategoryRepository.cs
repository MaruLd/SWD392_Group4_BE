using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;

namespace Persistence.Repositories
{
	public class EventCategoryRepository : BaseRepository<EventCategory, int>
	{
		public EventCategoryRepository(DataContext context) : base(context)
		{
		}
	}
}