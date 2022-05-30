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

namespace API.Services
{
	public class EventService
	{
		private EventRepository _eventRepository;
		private IMapper _mapper;

		public EventService(EventRepository eventRepository, DataContext context, IMapper mapper)
		{
			_eventRepository = eventRepository;
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
		public async Task<bool> Insert(Event e) => await _eventRepository.Insert(e);
		public async Task<bool> Update(Event e) => await _eventRepository.Update(e);
	}
}