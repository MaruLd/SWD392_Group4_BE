using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Application.Tickets.DTOs;
using Domain;
using Persistence;
using Persistence.Params;
using Persistence.Repositories;

namespace API.Services
{
	public class TicketService
	{
		private TicketRepository _ticketRepository;

		public TicketService(TicketRepository ticketRepository, DataContext context)
		{
			_ticketRepository = ticketRepository;
		}

		public async Task<List<Ticket>> Get(ListTicketDTO dto)
		{
			var query = _ticketRepository.GetQuery();

			if (dto.EventId != null)
			{
				query = query.Where(t => t.EventTicket.Any(et => et.EventId == dto.EventId));
			}

			if (dto.OrderBy == "Date")
			{
				query = query.OrderBy(ticket => ticket.CreatedDate);
			}

			return await query.OrderBy(e => e.CreatedDate).ToListAsync();
		}

		public async Task<List<Ticket>> GetAllFromEvent(Guid eventId)
		{
			var query = _ticketRepository.GetQuery();
			return await query.Where(t => t.EventTicket.Any(et => et.EventId == eventId)).OrderBy(e => e.CreatedDate).ToListAsync();
		}

		public async Task<Ticket> GetByID(Guid id) => await _ticketRepository.GetByID(id);
		public async Task<bool> Insert(Ticket e) => await _ticketRepository.Insert(e);
		public async Task<bool> Update(Ticket e) => await _ticketRepository.Update(e);
	}
}