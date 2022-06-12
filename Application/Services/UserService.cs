using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.Events.DTOs;
using Application.Users.DTOs;
using AutoMapper;
using Domain;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Repositories;

namespace Application.Services
{
	public class UserService
	{
		// private EventRepository _eventRepository;
		private UserRepository _userRepository;
		private IMapper _mapper;

		public UserService(
			UserRepository userRepository,
			IMapper mapper)
		{
			_userRepository = userRepository;
			_mapper = mapper;
		}

		public async Task<PagedList<User>> Get(UserQueryParams queryParams)
		{
			var query = _userRepository.GetQuery();

			if (queryParams.Email != null) query = query.Where(u => u.Email.ToLower().Contains(queryParams.Email.ToLower()));
			if (queryParams.DisplayName != null) query = query.Where(u => u.DisplayName.ToLower().Contains(queryParams.DisplayName.ToLower()));

			switch (queryParams.OrderBy)
			{
				case OrderByEnum.DateAscending:
					query = query.OrderBy(t => t.CreatedDate);
					break;
				case OrderByEnum.DateDescending:
					query = query.OrderByDescending(t => t.CreatedDate);
					break;
				default:
					break;
			}

			return await PagedList<User>.CreateAsync(query, queryParams.PageNumber, queryParams.PageSize);
		}

		public async Task<User> GetByEmail(string email) => await _userRepository.GetQuery().FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
		public async Task<User> GetByID(Guid id) => await _userRepository.GetByID(id);

		// public async Task<bool> Insert(Ticket e) { _ticketRepository.Insert(e); return await _ticketRepository.Save(); }
		public async Task<bool> Update(User e) { _userRepository.Update(e); return await _userRepository.Save(); }
		public async Task<bool> Save() { return await _userRepository.Save(); }
	}
}