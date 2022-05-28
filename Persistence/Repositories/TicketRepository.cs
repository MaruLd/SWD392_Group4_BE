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

		public async Task<List<Ticket>> Get(TicketParams param)
		{
			var query = _context.Ticket.AsQueryable();

			if (param.EventId != null)
			{
				query = query.Where(ticket => ticket.EventId == param.EventId);
			}

			if (param.OrderBy == "Date")
			{
				query = query.OrderBy(ticket => ticket.CreatedDate);
			}

			return await query.OrderBy(e => e.CreatedDate).ToListAsync();
		}		
		
		public async Task<List<Ticket>> GetAllFromEvent(int eventId)
		{
		
			return await _context.Ticket.Where(t => t.EventId == eventId).OrderBy(e => e.CreatedDate).ToListAsync();
		}


		public async Task<Ticket> GetByID(int id)
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