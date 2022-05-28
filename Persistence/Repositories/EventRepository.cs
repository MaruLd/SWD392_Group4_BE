using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.EntityFrameworkCore;
using Persistence.Params;

namespace Persistence.Repositories
{
	public class EventRepository
	{
		private DataContext _context;

		public EventRepository(DataContext context)
		{
			_context = context;
		}

		public async Task<List<Event>> Get(EventParams eventParams)
		{
			var query = _context.Event.AsQueryable();

			if (eventParams.Title != null) query = query.Where(e => e.Title.Contains(eventParams.Title));
			if (eventParams.StartTime != null) query = query.Where(e => e.StartTime > eventParams.StartTime);
			if (eventParams.EndTime != null) query = query.Where(e => e.EndTime > eventParams.EndTime);

			if (eventParams.OrderBy == "Date")
			{
				query = query.OrderBy(e => e.CreatedDate);
			}

			return await query.ToListAsync();
		}

		public async Task<Event> GetByID(int id)
		{
			return await _context.Event.FindAsync(id);
		}

		public async Task<bool> Insert(Event e)
		{
			_context.Event.Add(e);
			return await Save();
		}

		public async Task<bool> Update(Event e)
		{
			_context.Event.Update(e);
			return await Save();
		}

		public async Task<bool> Save()
		{
			return await _context.SaveChangesAsync() > 0;
		}
	}
}