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
	public class EventUserService
	{
		// private EventRepository _eventRepository;
		private EventUserRepository _eventUserRepository;

		private IMapper _mapper;

		public EventUserService(
			EventUserRepository eventUserRepository,
			IMapper mapper)
		{
			_eventUserRepository = eventUserRepository;
			_mapper = mapper;
		}

		public async Task<PagedList<EventUser>> Get(Guid eventId, EventUserQueryParams queryParams)
		{
			var query = _eventUserRepository.GetQuery();
			query = query.Include(e => e.Event).Include(e => e.User);
			query = query.Where(e => e.EventId == eventId);
			
			if (queryParams.Type != 0) query = query.Where(u => u.Type == queryParams.Type);
			if (queryParams.DisplayName != null) query = query.Where(u => u.User.DisplayName.ToLower().Contains(queryParams.DisplayName.ToLower()));
			if (queryParams.Email != null) query = query.Where(u => u.User.Email.ToLower().Contains(queryParams.Email.ToLower()));

			return await PagedList<EventUser>.CreateAsync(query, queryParams.PageNumber, queryParams.PageSize);
		}

		public async Task<EventUser> GetByID(Guid eventId, Guid userId)
		{
			var query = _eventUserRepository.GetQuery();
			query = query.Where(e => e.UserId == userId && e.EventId == eventId);

			return await query.FirstOrDefaultAsync();
		}

		public async Task<bool> Insert(EventUser e)
		{
			_eventUserRepository.Insert(e);
			return await _eventUserRepository.Save();
		}

		public async Task<bool> Update(EventUser e)
		{
			_eventUserRepository.Update(e);
			return await _eventUserRepository.Save();
		}

		public async Task<bool> Delete(EventUser e)
		{
			_eventUserRepository.Delete(e);
			return await _eventUserRepository.Save();
		}
		public async Task<bool> Save()
		{
			return await _eventUserRepository.Save();
		}
	}
}