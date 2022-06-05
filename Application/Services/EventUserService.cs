using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Events.DTOs;
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

		public async Task<List<EventUser>> Get()
		{
			return await _eventUserRepository.GetAll();
		}

		public async Task<EventUser> GetByID(Guid eventId, Guid userId) => await _eventUserRepository.GetByID(eventId, userId);

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
	}
}