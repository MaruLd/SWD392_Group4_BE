using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Application.Tickets.DTOs;
using Domain;
using Persistence;
using Persistence.Repositories;
using Application.Core;

namespace Application.Services
{
	public class TicketService
	{
		private TicketRepository _ticketRepository;

		public TicketService(TicketRepository ticketRepository, DataContext context)
		{
			_ticketRepository = ticketRepository;
		}

		public async Task<List<Ticket>> Get(TicketQueryParams ticketParams)
		{
			var query = _ticketRepository.GetQuery();

			if (ticketParams.EventId != null)
			{
				query = query.Where(t => t.EventId == ticketParams.EventId);
			}

			switch (ticketParams.OrderBy)
			{
				case OrderByEnum.DateAscending:
					query = query.OrderBy(t => t.CreatedDate);
					break;
				case OrderByEnum.DateDescending:
					query = query.OrderByDescending(t => t.CreatedDate);
					break;
				default:
					break;
			}

			var list = await PagedList<Ticket>.CreateAsync(query, ticketParams.PageNumber, ticketParams.PageSize);
			return list;
		}

		public async Task<List<Ticket>> GetAllFromEvent(Guid eventId)
		{
			var query = _ticketRepository.GetQuery();
			return await query.Where(t => t.EventId == eventId).OrderBy(e => e.CreatedDate).ToListAsync();
		}

		public async Task<Ticket> GetByID(Guid id) => await _ticketRepository.GetByID(id);
		public async Task<bool> Insert(Ticket e) { _ticketRepository.Insert(e); return await _ticketRepository.Save(); }
		public async Task<bool> Update(Ticket e) { _ticketRepository.Update(e); return await _ticketRepository.Save(); }
		public async Task<bool> Save() { return await _ticketRepository.Save(); }
	}
}