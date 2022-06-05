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


		public async Task<List<EventAgendaDTO>> GetAllByEventId(Guid eventId)
		{
			var ea = await _eventAgendaRepository.GetQuery()
				.Where(ea => ea.EventId == eventId)
				.ToListAsync();
				
			return  _mapper.Map<List<EventAgendaDTO>>(ea);
		}

		public async Task<List<EventAgendaDTO>> GetAll() => _mapper.Map<List<EventAgendaDTO>>(await _eventAgendaRepository.GetAll());
		public async Task<EventAgendaDTO> GetByID(Guid id)
		{
			var ec = await _eventAgendaRepository.GetByID(id);
			if (ec == null) return null;
			var ecDto = _mapper.Map<EventAgendaDTO>(ec);
			return ecDto;
		}


		public async Task<bool> Insert(EventAgenda e) { _eventAgendaRepository.Insert(e); return await _eventAgendaRepository.Save(); }
		public async Task<bool> Update(EventAgenda e) { _eventAgendaRepository.Update(e); return await _eventAgendaRepository.Save(); }
		public async Task<bool> Save() { return await _eventAgendaRepository.Save(); }
	}
}