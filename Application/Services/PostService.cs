using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Application.Posts.DTOs;
using Domain;
using Persistence;
using Persistence.Repositories;
using Domain.Enums;
using Application.Core;

namespace Application.Services
{
	public class PostService
	{
		private PostRepository _PostRepository;

		public PostService(PostRepository PostRepository, DataContext context)
		{
			_PostRepository = PostRepository;
		}

		public async Task<List<Post>> Get(PostQueryParams queryParams)
		{
			var query = _PostRepository.GetQuery();

			if (queryParams.EventId != Guid.Empty)
			{
				query = query.Where(t => t.EventId == queryParams.EventId);
			}

			if (queryParams.Title != null)
			{
				query = query.Where(t => t.Title.ToLower().Contains(queryParams.Title.ToLower()));
			}

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

			return await PagedList<Post>.CreateAsync(query, queryParams.PageNumber, queryParams.PageSize);
		}

		public async Task<List<Post>> GetAllFromEvent(Guid eventId)
		{
			var query = _PostRepository.GetQuery();
			return await query.Where(t => t.EventId == eventId).OrderBy(e => e.CreatedDate).ToListAsync();
		}

		public async Task<Post> GetByID(Guid id) => await _PostRepository.GetByID(id);
		public async Task<bool> Insert(Post e) { _PostRepository.Insert(e); return await _PostRepository.Save(); }
		public async Task<bool> Update(Post e) { _PostRepository.Update(e); return await _PostRepository.Save(); }
		public async Task<bool> Save() { return await _PostRepository.Save(); }
	}
}