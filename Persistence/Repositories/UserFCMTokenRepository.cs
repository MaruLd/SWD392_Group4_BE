using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{
	public class UserFCMTokenRepository : BaseRepository<UserFCMToken, Guid>
	{
		public UserFCMTokenRepository(DataContext context) : base(context)
		{
		}
	}
}