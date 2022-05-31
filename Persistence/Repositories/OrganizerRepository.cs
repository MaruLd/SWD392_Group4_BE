using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;

namespace Persistence.Repositories
{
	public class OrganizerRepository : BaseRepository<Organizer, Guid>
	{
		public OrganizerRepository(DataContext context) : base(context)
		{
		}
	}
}