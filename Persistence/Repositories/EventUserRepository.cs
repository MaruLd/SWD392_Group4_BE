using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{
	public class EventUserRepository : BaseRepository<EventUser, Guid>
	{
		public EventUserRepository(DataContext context) : base(context)
		{
		}

		public async Task<EventUser> GetByID(Guid eventId, Guid userId)
		{
			var query = GetQuery();
			return await
				query.Where(ue => ue.EventId == eventId && ue.UserId == userId).FirstOrDefaultAsync();
		}
	}
}