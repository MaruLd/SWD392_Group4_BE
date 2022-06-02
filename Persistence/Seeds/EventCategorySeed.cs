using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;

namespace Persistence
{
	public class EventCategorySeed
	{
		public static async Task SeedData(DataContext context)
		{
			if (context.EventCategories.Any()) return;


			context.EventCategories.Add(new EventCategory() { Name = "General" });
			await context.SaveChangesAsync();
		}
	}
}