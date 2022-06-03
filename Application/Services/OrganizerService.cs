using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Application.Tickets.DTOs;
using Domain;
using Persistence;
using Persistence.Repositories;
using Application.Organizers.DTOs;

namespace Application.Services
{
	public class OrganizerService
	{
		private OrganizerRepository _organizerRepository;

		public OrganizerService(OrganizerRepository organizerRepository)
		{
			_organizerRepository = organizerRepository;
		}

		public async Task<List<Organizer>> Get(OrganizerQueryParams dto)
		{
			var query = _organizerRepository.GetQuery();

			if (dto.Name != null)
			{
				query = query.Where(o => o.Name.Contains(dto.Name));
			}

			switch (dto.OrderBy)
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

			return await query.OrderBy(e => e.CreatedDate).ToListAsync();
		}

		public async Task<List<Organizer>> GetAll()
		{
			var query = _organizerRepository.GetQuery();
			return await query.OrderBy(e => e.CreatedDate).ToListAsync();
		}

		public async Task<Organizer> GetByID(Guid id) => await _organizerRepository.GetByID(id);
		public async Task<bool> Insert(Organizer e) { _organizerRepository.Insert(e); return await _organizerRepository.Save(); }
		public async Task<bool> Update(Organizer e) { _organizerRepository.Update(e); return await _organizerRepository.Save(); }
		public async Task<bool> Save() { return await _organizerRepository.Save(); }
	}
}