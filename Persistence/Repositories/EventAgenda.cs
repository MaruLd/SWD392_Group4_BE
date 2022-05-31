using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;

namespace Persistence.Repositories
{
	public class EventAgendaRepository : BaseRepository<EventAgenda, Guid>
	{
		public EventAgendaRepository(DataContext context) : base(context)
		{
		}
	}
}