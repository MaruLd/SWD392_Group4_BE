using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;

namespace Persistence.Repositories
{
	public class CommentRepository : BaseRepository<Comment, Guid>
	{
		public CommentRepository(DataContext context) : base(context)
		{
		}
	}
}