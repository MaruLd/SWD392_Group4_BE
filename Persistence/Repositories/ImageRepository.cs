using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{
	public class UserImageRepository : BaseRepository<UserImage, Guid>
	{
		public UserImageRepository(DataContext context) : base(context)
		{
		}
	}
}