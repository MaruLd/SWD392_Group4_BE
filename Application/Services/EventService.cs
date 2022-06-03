using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.Events.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Repositories;

namespace Application.Services
{
	public class EventService
	{
		// private EventRepository _eventRepository;
		private EventRepository _eventRepository;
		private EventUserRepository _eventUserRepository;

		private IMapper _mapper;

		public EventService(
			EventRepository eventRepository,
			EventUserRepository eventUserRepository,
			IMapper mapper)
		{
			_eventRepository = eventRepository;
			_eventUserRepository = eventUserRepository;
			_mapper = mapper;
		}

		public async Task<List<Event>> Get(EventQueryParams eventParams)
		{
			var query = _eventRepository.GetQuery();

			if (eventParams.Title != null) query = query.Where(e => e.Title.Contains(eventParams.Title));
			if (eventParams.StartTime != null) query = query.Where(e => e.StartTime > eventParams.StartTime);
			if (eventParams.EndTime != null) query = query.Where(e => e.EndTime > eventParams.EndTime);
			switch (eventParams.OrderBy)
			{
				case OrderByEnum.DateAscending:
					query = query.OrderBy(e => e.CreatedDate);
					break;
				case OrderByEnum.DateDescending:
					query = query.OrderByDescending(e => e.CreatedDate);
					break;
				default:
					break;
			}

			query = query.Include(e => e.Tickets).Include(e => e.Organizers);
			if (eventParams.OrganizerId != Guid.Empty) query = query.Where(e => e.Organizers.Any(o => o.Id == eventParams.OrganizerId));


			var list = await PagedList<Event>.CreateAsync(query, eventParams.PageNumber, eventParams.PageSize);

			return list;
		}

		public async Task<Event> GetByID(Guid id) => await _eventRepository.GetByID(id);

		public async Task<bool> CreateEvent(CreateEventDTO e, Guid userId)
		{
			var eventEntity = _mapper.Map<Event>(e);
			_eventRepository.Insert(eventEntity);

			if (!await _eventRepository.Save()) return false;

			_eventUserRepository.Insert(new EventUser()
			{
				EventId = eventEntity.Id,
				UserId = userId,
				Type = EventUserType.Manager,
				Status = EventUserStatus.Attended,
			});


			if (!await _eventUserRepository.Save()) return false;
			return true;
		}

		public async Task<bool> Update(Event e)
		{
			_eventRepository.Update(e);
			return await _eventRepository.Save();
		}

		public async Task<bool> Save()
		{
			return await _eventRepository.Save();
		}
	}
}
