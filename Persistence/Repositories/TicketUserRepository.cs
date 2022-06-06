using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{
	public class TicketUserRepository : BaseRepository<TicketUser, Guid>
	{
		public TicketUserRepository(DataContext context) : base(context)
		{
		}

		public async Task<TicketUser> GetByID(Guid eventId, Guid userId)
		{
			var query = GetQuery();
			return await
				query.Where(tu => tu.TicketId == eventId && tu.UserId == userId).FirstOrDefaultAsync();
		}
	}
}