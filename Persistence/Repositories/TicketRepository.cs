using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.EntityFrameworkCore;
using Persistence.Params;

namespace Persistence.Repositories
{
	public class TicketRepository
	{
		private DataContext _context;

		public TicketRepository(DataContext context)
		{
			_context = context;
		}

		public IQueryable<Ticket> GetQuery()
		{
			return _context.Ticket.AsQueryable();
		}

		public async Task<List<Ticket>> GetAll()
		{
			return await _context.Ticket.ToListAsync();
		}

		public async Task<Ticket> GetByID(Guid id)
		{
			return await _context.Ticket.FindAsync(id);
		}

		public async Task<bool> Insert(Ticket e)
		{
			_context.Ticket.Add(e);
			return await Save();
		}

		public async Task<bool> Update(Ticket e)
		{
			_context.Ticket.Update(e);
			return await Save();
		}

		public async Task<bool> Save()
		{
			return await _context.SaveChangesAsync() > 0;
		}
	}
}