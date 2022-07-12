using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{
	public class EventCodeRepository : BaseRepository<EventCode, Guid>
	{
		public EventCodeRepository(DataContext context) : base(context)
		{
		}
	}
}