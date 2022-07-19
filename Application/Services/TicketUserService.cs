using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.Events.DTOs;
using Application.EventUsers.DTOs;
using Application.TicketUsers.DTOs;
using Application.Users.DTOs;
using AutoMapper;
using Domain;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Repositories;

namespace Application.Services
{
	public class TicketUserService
	{
		private readonly TicketUserRepository _ticketUserRepository;
		private readonly TicketRepository _ticketRepository;
		private IMapper _mapper;

		public TicketUserService(
			TicketUserRepository ticketUserRepository,
			TicketRepository ticketRepository,
			IMapper mapper)
		{
			this._ticketUserRepository = ticketUserRepository;
			this._ticketRepository = ticketRepository;
			_mapper = mapper;
		}

		public async Task<List<TicketUser>> Get(Guid ticketId)
		{
			return await Get(ticketId, new TicketUserQueryParams());
		}

		public async Task<PagedList<TicketUser>> Get(Guid ticketId, TicketUserQueryParams queryParams)
		{
			var query = _ticketUserRepository.GetQuery();
			query = query.Include(e => e.Ticket).Include(e => e.User);
			query = query.Where(e => e.TicketId == ticketId).OrderByDescending(entity => entity.CreatedDate);

			if (queryParams.DisplayName != null) query = query.Where(u => u.User.DisplayName.ToLower().Contains(queryParams.DisplayName.ToLower()));
			if (queryParams.Email != null) query = query.Where(u => u.User.Email.ToLower().Contains(queryParams.Email.ToLower()));

			return await PagedList<TicketUser>.CreateAsync(query, queryParams.PageNumber, queryParams.PageSize);
		}

		public async Task<TicketUser> GetByID(Guid ticketId, Guid userId)
		{
			return await _ticketUserRepository.GetQuery()
			.Where(e => e.UserId == userId && e.TicketId == ticketId)
			.Include(e => e.Ticket).Include(e => e.User).FirstOrDefaultAsync();
		}

		public async Task<TicketUser> GetTicketUserInEvent(Guid eventId, Guid userId)
		{
			return await _ticketUserRepository.GetQuery()
			.Include(tu => tu.Ticket).Where(tu => tu.Ticket.EventId == eventId)
			.Include(tu => tu.User).Where(tu => tu.UserId == userId).FirstOrDefaultAsync();
		}

		public async Task<PagedList<TicketUser>> GetTicketsFromUser(Guid userId, TickerUserSelfQueryParams queryParams)
		{
			var query = _ticketUserRepository.GetQuery()
			.Include(tu => tu.Ticket).ThenInclude(tu => tu.Event).Where(tu => tu.UserId == userId);
			if (queryParams.ticketUserStateEnum != TicketUserStateEnum.None)
			{
				query = query.Where(t => t.State == queryParams.ticketUserStateEnum);
			}
			if (queryParams.EventId != Guid.Empty) {
				query = query.Where(t => t.Ticket.EventId == queryParams.EventId);
			}
			query = query.OrderByDescending(entity => entity.CreatedDate);

			return await PagedList<TicketUser>.CreateAsync(query, queryParams.PageNumber, queryParams.PageSize);
		}

		public async Task<bool> Insert(TicketUser e)
		{
			_ticketUserRepository.Insert(e);
			return await _ticketUserRepository.Save();
		}

		public async Task<bool> Update(TicketUser e)
		{
			_ticketUserRepository.Update(e);
			return await _ticketUserRepository.Save();
		}
		public async Task<bool> Remove(TicketUser e)
		{
			_ticketUserRepository.Delete(e);
			return await _ticketUserRepository.Save();
		}

		public async Task<bool> Save()
		{
			return await _ticketUserRepository.Save();
		}
	}
}