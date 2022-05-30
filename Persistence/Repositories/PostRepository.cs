using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;

namespace Persistence.Repositories
{
	public class PostRepository : BaseRepository<Post, Guid>
	{
		public PostRepository(DataContext context) : base(context)
		{
		}
	}
}