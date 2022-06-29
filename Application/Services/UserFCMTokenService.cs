using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Persistence;
using Persistence.Repositories;
using Domain.Enums;
using Application.Core;

namespace Application.Services
{
	public class UserFCMTokenService
	{
		private readonly UserFCMTokenRepository _userFCMTokenRepository;

		public UserFCMTokenService(UserFCMTokenRepository userFCMTokenRepository)
		{
			this._userFCMTokenRepository = userFCMTokenRepository;
		}

		public async Task<List<UserFCMToken>> GetAllFromUser(Guid userId)
		{
			var query = _userFCMTokenRepository.GetQuery();
			return await query.Where(t => t.UserId == userId).OrderByDescending(e => e.CreatedDate).ToListAsync();
		}

		public async Task<UserFCMToken> GetByFCMToken(string token)
		{
			var query = _userFCMTokenRepository.GetQuery();
			return await query.Where(t => t.Token == token).OrderByDescending(e => e.CreatedDate).FirstOrDefaultAsync();
		}

		public async Task<UserFCMToken> GetById(Guid id)
		{
			var query = _userFCMTokenRepository.GetQuery();
			return await query.Where(t => t.Id == id).OrderByDescending(e => e.CreatedDate).FirstOrDefaultAsync();
		}

		public async Task<UserFCMToken> GetByID(Guid id) => await _userFCMTokenRepository.GetByID(id);
		public async Task<bool> Insert(UserFCMToken e) { _userFCMTokenRepository.Insert(e); return await _userFCMTokenRepository.Save(); }
		public async Task<bool> Update(UserFCMToken e) { _userFCMTokenRepository.Update(e); return await _userFCMTokenRepository.Save(); }
		public async Task<bool> Save() { return await _userFCMTokenRepository.Save(); }
	}
}