using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Persistence.Repositories
{
	public class BaseRepository<T, K> where T : class
	{
		private DataContext _context;
		private DbSet<T> table;

		public BaseRepository(DataContext context)
		{
			_context = context;
			table = _context.Set<T>();
		}

		public IQueryable<T> GetQuery()
		{
			return table.AsQueryable();
		}

		public async Task<List<T>> GetAll()
		{
			return await table.ToListAsync();
		}

		public async Task<T> GetByID(K id)
		{
			return await table.FindAsync(id);
		}

		public void Insert(T e)
		{
			table.Add(e);
		}

		public void Update(T e)
		{
			table.Update(e);
		}

		public void Delete(T e)
		{
			table.Remove(e);
		}

		public async Task<bool> Save()
		{
			return await _context.SaveChangesAsync() > 0;
		}
	}
}