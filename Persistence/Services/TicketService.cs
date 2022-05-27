using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Persistence.Repositories;

namespace Persistence.Services
{
	public class TicketService
	{
		private TicketRepository _ticketRepository;

		public TicketService(TicketRepository ticketRepository)
		{
			_ticketRepository = ticketRepository;
		}

		public async Task<IEnumerable<Ticket>> GetAll()
		{
			return await _ticketRepository.GetAll();
		}

		public async Task<Ticket> GetByID(int id)
		{
			return await _ticketRepository.GetByID(id);
		}

		public async Task<bool> Insert(Ticket e)
		{
			return await _ticketRepository.Insert(e);
		}

		public async Task<bool> Update(Ticket e)
		{
			return await _ticketRepository.Update(e);
		}

		public async Task<bool> Save()
		{
			return await _ticketRepository.Save();
		}
	}
}