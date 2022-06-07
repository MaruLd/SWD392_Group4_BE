using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Events.DTOs;
using Application.EventUsers.DTOs;
using Application.TicketUsers.DTOs;
using AutoMapper;
using Domain;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Repositories;

namespace Application.Services
{
	public class EventOrganizerService
	{
		private readonly EventOrganizerRepository _eventOrganizerRepository;
		private readonly EventRepository _eventRepository;
		private IMapper _mapper;

		public EventOrganizerService(
			EventOrganizerRepository eventOrganizerRepository,
			EventRepository eventRepository,
			IMapper mapper)
		{
			this._eventOrganizerRepository = eventOrganizerRepository;
			this._eventRepository = eventRepository;
			_mapper = mapper;
		}


		public async Task<List<EventOrganizer>> Get(Guid eventId)
		{
			var query = _eventOrganizerRepository.GetQuery();
			query = query.Include(e => e.Event).Include(e => e.Organizer);
			query = query.Where(e => e.EventId == eventId);

			return await query.ToListAsync();
		}

		public async Task<EventOrganizer> GetByID(Guid eventId, Guid organizerId)
		{
			return await _eventOrganizerRepository.GetQuery()
			.Where(e => e.EventId == eventId && e.OrganizerId == organizerId)
			.Include(e => e.Event).Include(e => e.Organizer).FirstOrDefaultAsync();
		}

		public async Task<bool> Insert(EventOrganizer e)
		{
			_eventOrganizerRepository.Insert(e);
			return await _eventOrganizerRepository.Save();
		}

		public async Task<bool> Update(EventOrganizer e)
		{
			_eventOrganizerRepository.Update(e);
			return await _eventOrganizerRepository.Save();
		}
		public async Task<bool> Delete(EventOrganizer e)
		{
			_eventOrganizerRepository.Delete(e); return await _eventOrganizerRepository.Save();
		}
		public async Task<bool> Save()
		{
			return await _eventOrganizerRepository.Save();
		}
	}
}