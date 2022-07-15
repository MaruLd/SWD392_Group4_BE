using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{
	public class LocationRepository : BaseRepository<Location, Guid>
	{
		public LocationRepository(DataContext context) : base(context)
		{
		}
	}
}