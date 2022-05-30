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

		public IQueryable<Event> GetQuery()
		{
			return _context.Event.AsQueryable();
		}

		public async Task<List<Event>> GetAll()
		{
			return await _context.Event.ToListAsync();
		}

		public async Task<Event> GetByID(Guid id)
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