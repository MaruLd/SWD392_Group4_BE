using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Persistence;
using Persistence.Repositories;
using Domain.Enums;
using Application.Core;
using Application.Locations.DTOs;

namespace Application.Services
{
	public class LocationService
	{
		private readonly LocationRepository _locationRepository;

		public LocationService(LocationRepository locationRepository)
		{
			this._locationRepository = locationRepository;
		}

		public async Task<List<Location>> Get(LocationQueryParams queryParams)
		{
			var query = _locationRepository.GetQuery();
			
			if (queryParams.Name != null)
			{
				query = query.Where(t => t.Name.ToLower().Contains(queryParams.Name.ToLower()));
			}

			return await query.OrderBy(e => e.Name).ToListAsync();
		}

		public async Task<Location> GetById(Guid id)
		{
			var query = _locationRepository.GetQuery();
			return await query.Where(t => t.Id == id).FirstOrDefaultAsync();
		}

		public async Task<Location> GetByID(Guid id) => await _locationRepository.GetByID(id);
		public async Task<bool> Insert(Location e) { _locationRepository.Insert(e); return await _locationRepository.Save(); }
		public async Task<bool> Update(Location e) { _locationRepository.Update(e); return await _locationRepository.Save(); }
		public async Task<bool> Save() { return await _locationRepository.Save(); }
	}
}