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
using Application.Core;
using Domain.Enums;
using Application.EventCategory.DTOs;
using AutoMapper;

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

		public async Task<List<EventCategoryDTO>> GetAll() => _mapper.Map<List<EventCategoryDTO>>(await _eventCategoryRepository.GetAll());
		public async Task<EventCategoryDTO> GetByID(int id)
		{
			var ec = await _eventCategoryRepository.GetByID(id);
			if (ec == null) return null;
			var ecDto = _mapper.Map<EventCategoryDTO>(ec);

			int eCounts = await _eventRepository.GetQuery().Where(e => e.EventCategoryId == ec.Id).CountAsync();
			ecDto.EventCount = eCounts;
			return ecDto;
		}

		public async Task<bool> Insert(Domain.EventCategory e) { _eventCategoryRepository.Insert(e); return await _eventCategoryRepository.Save(); }
		public async Task<bool> Update(Domain.EventCategory e) { _eventCategoryRepository.Update(e); return await _eventCategoryRepository.Save(); }
		public async Task<bool> Save() { return await _eventCategoryRepository.Save(); }
	}
}