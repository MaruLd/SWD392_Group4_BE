using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{
	public class UserRepository : BaseRepository<User, Guid>
	{
		public UserRepository(DataContext context) : base(context)
		{
		}
	}
}