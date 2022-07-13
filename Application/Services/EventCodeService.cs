using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.Events.DTOs;
using Application.EventUsers.DTOs;
using AutoMapper;
using Domain;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Repositories;

namespace Application.Services
{
	public class EventCodeService
	{
		private EventCodeRepository _eventCodeRepository;

		private IMapper _mapper;

		public EventCodeService(
			EventCodeRepository eventCodeRepository,
			IMapper mapper)
		{
			_eventCodeRepository = eventCodeRepository;
			_mapper = mapper;
		}

		public async Task<EventCode> GetByID(Guid id)
		{
			var query = _eventCodeRepository.GetQuery();
			query = query.Where(e => e.Id == id);

			return await query.FirstOrDefaultAsync();
		}

		public async Task<EventCode> GetByEventID(Guid id)
		{
			var query = _eventCodeRepository.GetQuery();
			query = query.Where(e => e.EventId == id);

			return await query.FirstOrDefaultAsync();
		}
		
		public async Task<EventCode> GetByCode(string code)
		{
			var query = _eventCodeRepository.GetQuery();
			query = query.Where(e => e.Code == code);

			return await query.FirstOrDefaultAsync();
		}

		public async Task<bool> Insert(EventCode e)
		{
			_eventCodeRepository.Insert(e);
			return await _eventCodeRepository.Save();
		}

		public async Task<bool> Update(EventCode e)
		{
			_eventCodeRepository.Update(e);
			return await _eventCodeRepository.Save();
		}

		public async Task<bool> Delete(EventCode e)
		{
			_eventCodeRepository.Delete(e);
			return await _eventCodeRepository.Save();
		}
		public async Task<bool> Save()
		{
			return await _eventCodeRepository.Save();
		}
	}
}