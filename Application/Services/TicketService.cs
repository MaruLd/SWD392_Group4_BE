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
using Domain.Enums;

namespace Application.Services
{
	public class TicketService
	{
		private TicketRepository _ticketRepository;

		public TicketService(TicketRepository ticketRepository, DataContext context)
		{
			_ticketRepository = ticketRepository;
		}

		public async Task<PagedList<Ticket>> Get(TicketQueryParams ticketParams)
		{
			var query = _ticketRepository.GetQuery();
			query = query.Where(e => e.Status != StatusEnum.Unavailable);
			query = query.Include(t => t.TicketUsers).ThenInclude(tu => tu.User);

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

			return await PagedList<Ticket>.CreateAsync(query, ticketParams.PageNumber, ticketParams.PageSize);
		}

		public async Task<List<Ticket>> GetAllFromEvent(Guid eventId, bool withUsers = false)
		{
			var query = _ticketRepository.GetQuery();
			query = query.Where(t => t.EventId == eventId);
			if (withUsers)
			{
				query = query.Include(t => t.TicketUsers).ThenInclude(tu => tu.User);
			}

			return await query.OrderBy(e => e.CreatedDate).ToListAsync();
		}

		public async Task<Ticket> GetByID(Guid id)
		{
			var query = _ticketRepository.GetQuery();
			return await query.Where(t => t.Id == id)
			.Include(t => t.Event)
			.Include(t => t.TicketUsers).ThenInclude(tu => tu.User)
			.FirstOrDefaultAsync();
		}

		public async Task<bool> Insert(Ticket e) { _ticketRepository.Insert(e); return await _ticketRepository.Save(); }
		public async Task<bool> Update(Ticket e) { _ticketRepository.Update(e); return await _ticketRepository.Save(); }
		public async Task<bool> Save() { return await _ticketRepository.Save(); }
	}
}