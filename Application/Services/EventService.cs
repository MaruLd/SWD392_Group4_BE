using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Events.DTOs;
using AutoMapper;
using Domain;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Params;
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

		public async Task<List<EventDTO>> Get(ListEventDTO dto)
		{
			var query = _eventRepository.GetQuery();

			if (dto.Title != null) query = query.Where(e => e.Title.Contains(dto.Title));
			if (dto.StartTime != null) query = query.Where(e => e.StartTime > dto.StartTime);
			if (dto.EndTime != null) query = query.Where(e => e.EndTime > dto.EndTime);

			if (dto.OrderBy == "Date")
			{
				query = query.OrderBy(e => e.CreatedDate);
			}

			var list = await query
				.Include(e => e.EventTicket)
				.ThenInclude(et => et.Ticket)
				.ToListAsync();

			var eventDtos = _mapper.Map<List<EventDTO>>(list);
			// foreach (var e in eventDtos)
			// {
			// 	var tickets = await _ticketService.GetAllFromEvent(e.Id);
			// 	e.Tickets = tickets;
			// }

			// return list;
			return eventDtos;
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
				Status = EventUserStatus.ATTENDED,
			});


			if (!await _eventUserRepository.Save()) return false;
			return true;
		}

		public async Task<bool> Update(Event e)
		{
			_eventRepository.Update(e);
			return await _eventRepository.Save();
		}
	}
}