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
using AutoMapper;
using Application.EventAgendas.DTOs;
using AutoMapper.QueryableExtensions;

namespace Application.Services
{
	public class EventAgendaService
	{
		private readonly EventRepository _eventRepository;
		private readonly EventAgendaRepository _eventAgendaRepository;
		private readonly IMapper _mapper;

		public EventAgendaService(EventRepository eventRepository, EventAgendaRepository eventAgendaRepository, IMapper mapper)
		{
			_eventRepository = eventRepository;
			_eventAgendaRepository = eventAgendaRepository;
			_mapper = mapper;
		}

		public async Task<PagedList<EventAgenda>> Get(Guid eventId, EventAgendaQueryParams eventAgendaQueryParams)
		{
			var query = _eventAgendaRepository.GetQuery();
			query = query.Where(e => e.EventId == eventId);

			if (eventAgendaQueryParams.StartTime != null) query = query.Where(e => e.StartTime > eventAgendaQueryParams.StartTime);
			if (eventAgendaQueryParams.EndTime != null) query = query.Where(e => e.EndTime > eventAgendaQueryParams.EndTime);


			switch (eventAgendaQueryParams.OrderBy)
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

			var list = await PagedList<EventAgenda>.CreateAsync(query, eventAgendaQueryParams.PageNumber, eventAgendaQueryParams.PageSize);
			return list;
		}

		public async Task<List<EventAgenda>> GetAllByEventId(Guid eventId)
		{
			return await _eventAgendaRepository.GetQuery()
				.Where(ea => ea.EventId == eventId)
				.ToListAsync();

		}

		public async Task<List<EventAgenda>> GetAll() => _mapper.Map<List<EventAgenda>>(await _eventAgendaRepository.GetAll());
		public async Task<EventAgenda> GetByID(Guid id)
		{
			var ea = await _eventAgendaRepository.GetByID(id);
			if (ea == null) return null;
			return ea;
		}


		public async Task<bool> Insert(EventAgenda e) { _eventAgendaRepository.Insert(e); return await _eventAgendaRepository.Save(); }
		public async Task<bool> Update(EventAgenda e) { _eventAgendaRepository.Update(e); return await _eventAgendaRepository.Save(); }
		public async Task<bool> Delete(EventAgenda e) { _eventAgendaRepository.Delete(e); return await _eventAgendaRepository.Save(); }

		public async Task<bool> Save() { return await _eventAgendaRepository.Save(); }
	}
}