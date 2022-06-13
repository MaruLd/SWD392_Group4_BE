using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.Events.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain;
using Domain.Enums;
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
		private readonly EventOrganizerRepository _eventOrganizerRepository;
		private readonly UserRepository _userRepository;
		private readonly IUserAccessor _userAccessor;
		private IMapper _mapper;

		public EventService(
			EventRepository eventRepository,
			EventUserRepository eventUserRepository,
			EventOrganizerRepository eventOrganizerRepository,
			UserRepository userRepository,
			IUserAccessor userAccessor,
			IMapper mapper)
		{
			_eventRepository = eventRepository;
			_eventUserRepository = eventUserRepository;
			this._eventOrganizerRepository = eventOrganizerRepository;
			this._userRepository = userRepository;
			_userAccessor = userAccessor;
			_mapper = mapper;
		}

		public async Task<PagedList<Event>> Get(EventQueryParams eventParams)
		{
			var query = _eventRepository.GetQuery();
			query = query.Where(e => e.Status != StatusEnum.Unavailable);

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

			query = query
				.Include(e => e.EventOrganizers).ThenInclude(eo => eo.Organizer)
				.Include(e => e.EventCategory);

			var currentUserId = _userAccessor.GetID();
			if (eventParams.IsOwn && currentUserId != Guid.Empty)
			{
				query = query.Include(e => e.EventUsers)
				.Where(e => e.EventUsers.Any(eu => eu.UserId == currentUserId && eu.Type == EventUserTypeEnum.Creator));
			}

			if (eventParams.IsJoined && currentUserId != Guid.Empty)
			{
				query = query.Include(e => e.EventUsers)
				.Where(e => e.EventUsers.Any(eu => eu.UserId == currentUserId));
			}

			// Query Param [eventStateEnum]
			// If param existed
			if (eventParams.eventStateEnum != EventStateEnum.None)
			{
				// If the param is DRAFT
				if (eventParams.eventStateEnum == EventStateEnum.Draft)
				{
					// Check if user is logged in
					if (currentUserId != Guid.Empty)
					{
						query = query.Include(e => e.EventUsers).Where(e =>
							e.State == EventStateEnum.Draft &&
							e.EventUsers.Any(eu => eu.UserId == currentUserId && eu.Type == EventUserTypeEnum.Creator));
					}
					// If there is no user, don't show them anything
					else
					{
						query = query.Include(e => e.EventUsers).Where(e =>
							e.State == EventStateEnum.None);
					}
				}
				// If the param is exist and not DRAFT
				else
				{
					query = query.Include(e => e.EventUsers).Where(e => e.State == eventParams.eventStateEnum);
				}
			}
			// If there is no param, don't send DRAFT events!
			else
			{
				query = query.Include(e => e.EventUsers).Where(e => e.State != EventStateEnum.Draft);
			}


			if (eventParams.OrganizerName != null)
			{
				query = query.Where(e => e.EventOrganizers
					.Any(o =>
						o.Organizer.Name.ToLower().Contains(eventParams.OrganizerName) &&
						o.EventId == e.Id)
					);
			};

			if (eventParams.CategoryId != 0) query = query.Where(e => e.EventCategoryId == eventParams.CategoryId);

			var list = await PagedList<Event>.CreateAsync(query, eventParams.PageNumber, eventParams.PageSize);
			return list;
		}

		public async Task<Event> GetByID(Guid id)
		{
			var query = _eventRepository.GetQuery()
				.Where(entity => entity.Status != StatusEnum.Unavailable)
				.Where(e => e.Id == id);

			var currentUserId = _userAccessor.GetID();
			if (currentUserId != Guid.Empty)
			{
				query = query.Include(e => e.EventUsers)
					.Where(e => e.State != EventStateEnum.Draft
					|| (
						e.State == EventStateEnum.Draft && e.EventUsers.Any(eu => eu.UserId == currentUserId && eu.Type == EventUserTypeEnum.Creator)
						)
					);
			}

			var e = await query
				.Include(e => e.EventCategory)
				.Include(e => e.EventOrganizers).ThenInclude(eo => eo.Organizer)
				.Include(e => e.Tickets).ThenInclude(t => t.TicketUsers)
				.FirstOrDefaultAsync();
			return e;
		}

		public async Task<Event> CreateEvent(CreateEventDTO e, Guid userId)
		{
			var eventEntity = _mapper.Map<Event>(e);
			_eventRepository.Insert(eventEntity);

			if (!await _eventRepository.Save()) return null;

			_eventUserRepository.Insert(new EventUser()
			{
				EventId = eventEntity.Id,
				UserId = userId,
				Type = EventUserTypeEnum.Creator,
				Status = StatusEnum.Available,
			});


			if (!await _eventUserRepository.Save()) return null;
			return eventEntity;
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
