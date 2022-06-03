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

		public async Task<User> GetByEmail(string email)
		{
			return await _userRepository.GetQuery().FirstOrDefaultAsync(u => u.Email.ToLower() == email);
		}



		public async Task<User> GetByID(Guid id) => await _userRepository.GetByID(id);
	}
}