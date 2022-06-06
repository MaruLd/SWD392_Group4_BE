using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

using Persistence.Repositories;
using AutoMapper;
using Domain;

namespace Application.Services
{
	public class EventCategoryService
	{
		private readonly EventRepository _eventRepository;
		private EventCategoryRepository _eventCategoryRepository;
		private readonly IMapper _mapper;

		public EventCategoryService(EventRepository eventRepository, EventCategoryRepository eventCategoryRepository, IMapper mapper)
		{
			_eventRepository = eventRepository;
			_eventCategoryRepository = eventCategoryRepository;
			_mapper = mapper;
		}

		public async Task<List<EventCategory>> GetAll() => _mapper.Map<List<EventCategory>>(await _eventCategoryRepository.GetAll());
		public async Task<EventCategory> GetByID(int id)
		{
			var ec = await _eventCategoryRepository.GetQuery().Where(c => c.Id == id).Include(c => c.Events).FirstOrDefaultAsync();
			if (ec == null) return null;
			return ec;
		}

		public async Task<bool> Insert(EventCategory e) { _eventCategoryRepository.Insert(e); return await _eventCategoryRepository.Save(); }
		public async Task<bool> Update(EventCategory e) { _eventCategoryRepository.Update(e); return await _eventCategoryRepository.Save(); }
		public async Task<bool> Delete(EventCategory e) { _eventCategoryRepository.Delete(e); return await _eventCategoryRepository.Save(); }
		public async Task<bool> Save() { return await _eventCategoryRepository.Save(); }
	}
}