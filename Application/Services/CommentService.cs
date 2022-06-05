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

namespace Application.Services
{
	public class CommentService
	{
		private CommentRepository _commentRepository;

		public CommentService(CommentRepository commentRepository, DataContext context)
		{
			_commentRepository = commentRepository;
		}

		public async Task<List<Comment>> Get(Guid postId, CommentQueryParams queryParams)
		{
			var query = _commentRepository.GetQuery();
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

			return await query.OrderBy(e => e.CreatedDate).ToListAsync();
		}

		public async Task<List<Comment>> GetAllFromPost(Guid postId)
		{
			var query = _commentRepository.GetQuery();
			return await query.Where(t => t.PostId == postId).OrderBy(e => e.CreatedDate).ToListAsync();
		}

		public async Task<Comment> GetByID(Guid id) => await _commentRepository.GetByID(id);
		public async Task<bool> Insert(Comment e) { _commentRepository.Insert(e); return await _commentRepository.Save(); }
		public async Task<bool> Update(Comment e) { _commentRepository.Update(e); return await _commentRepository.Save(); }
		public async Task<bool> Save() { return await _commentRepository.Save(); }
	}
}