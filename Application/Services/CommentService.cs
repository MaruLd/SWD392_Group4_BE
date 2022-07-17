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
using Application.Comments.DTOs;
using Application.Core;

namespace Application.Services
{
	public class CommentService
	{
		private CommentRepository _commentRepository;

		public CommentService(CommentRepository commentRepository, DataContext context)
		{
			_commentRepository = commentRepository;
		}

		public async Task<PagedList<Comment>> Get(Guid postId, CommentQueryParams queryParams)
		{
			var query = _commentRepository.GetQuery();
			query = query.Where(e => e.Status != StatusEnum.Unavailable);
			query = query.Where(t => t.PostId == postId);

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

			query = query.Include(comment => comment.User);
			return await PagedList<Comment>.CreateAsync(query, queryParams.PageNumber, queryParams.PageSize);
		}

		public async Task<List<Comment>> GetAllFromPost(Guid postId)
		{
			var query = _commentRepository.GetQuery();
			return await query.Where(entity => entity.Status != StatusEnum.Unavailable).Where(t => t.PostId == postId).OrderBy(e => e.CreatedDate).ToListAsync();
		}

		public async Task<Comment> GetByID(Guid id)
		{
			return await _commentRepository.GetQuery().Where(e => e.Id == id).Include(c => c.Post).ThenInclude(p => p.Event).FirstOrDefaultAsync();
		}
		public async Task<bool> Insert(Comment e) { _commentRepository.Insert(e); return await _commentRepository.Save(); }
		public async Task<bool> Update(Comment e) { _commentRepository.Update(e); return await _commentRepository.Save(); }
		public async Task<bool> Save() { return await _commentRepository.Save(); }
	}
}