// using System;
// using System.Collections.Generic;
// using Microsoft.EntityFrameworkCore;
// using System.Linq;
// using System.Threading.Tasks;
// using Application.Tickets.DTOs;
// using Domain;
// using Persistence;
// using Persistence.Params;
// using Persistence.Repositories;
// using Application.Organizers.DTOs;

// namespace Application.Services
// {
// 	public class OrganizerService
// 	{
// 		private OrganizerRepository _organizerRepository;

// 		public OrganizerService(OrganizerRepository organizerRepository)
// 		{
// 			_organizerRepository = organizerRepository;
// 		}

// 		public async Task<List<Ticket>> Get(OrganizerQueryParams dto)
// 		{
// 			var query = _organizerRepository.GetQuery();

// 			if (dto.EventId != null)
// 			{
// 				query = query.Where(t => t.EventId == dto.EventId);
// 			}

// 			switch (dto.OrderBy)
// 			{
// 				case OrderByEnum.DateAscending:
// 					query = query.OrderBy(t => t.CreatedDate);
// 					break;
// 				case OrderByEnum.DateDescending:
// 					query = query.OrderByDescending(t => t.CreatedDate);
// 					break;
// 				default:
// 					break;
// 			}

// 			return await query.OrderBy(e => e.CreatedDate).ToListAsync();
// 		}

// 		public async Task<List<Ticket>> GetAllFromEvent(Guid eventId)
// 		{
// 			var query = _ticketRepository.GetQuery();
// 			return await query.Where(t => t.EventId == eventId).OrderBy(e => e.CreatedDate).ToListAsync();
// 		}

// 		public async Task<Ticket> GetByID(Guid id) => await _ticketRepository.GetByID(id);
// 		public async Task<bool> Insert(Ticket e) { _ticketRepository.Insert(e); return await _ticketRepository.Save(); }
// 		public async Task<bool> Update(Ticket e) { _ticketRepository.Update(e); return await _ticketRepository.Save(); }
// 		public async Task<bool> Save() { return await _ticketRepository.Save(); }
// 	}
// }