using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;

namespace Persistence
{
	public class EventSeed
	{
		public static async Task SeedData(DataContext context)
		{
			if (context.Event.Any()) return;

			var events = new List<Event>
			{
				new Event
				{
					Title = "Event 1",
					StartTime = DateTime.Now.AddMonths(1),
					EndTime = DateTime.Now.AddMonths(1).AddHours(2),
					Description = "Description about the event",
					EventCategoryId = 1,
					Multiplier_Factor = 1,
					Status = "AVAILABLE"
				},
				 new Event
				{
					Title = "Event 2",
					StartTime = DateTime.Now.AddMonths(1),
					EndTime = DateTime.Now.AddMonths(1).AddHours(2),
					Description = "Description about the event",
					EventCategoryId = 1,
					Multiplier_Factor = 1,
					Status = "AVAILABLE"
				},
				 new Event
				{
					Title = "Event 3",
					StartTime = DateTime.Now.AddMonths(1),
					EndTime = DateTime.Now.AddMonths(1).AddHours(2),
					Description = "Description about the event",
					EventCategoryId = 1,
					Multiplier_Factor = 1,
					Status = "AVAILABLE"
				},
				 new Event
				{
					Title = "Event 4",
					StartTime = DateTime.Now.AddMonths(1),
					EndTime = DateTime.Now.AddMonths(1).AddHours(2),
					Description = "Description about the event",
					EventCategoryId = 1,
					Multiplier_Factor = 1,
					Status = "AVAILABLE"
				},
				 new Event
				{
					Title = "Event 5",
					StartTime = DateTime.Now.AddMonths(1),
					EndTime = DateTime.Now.AddMonths(1).AddHours(2),
					Description = "Description about the event",
					EventCategoryId = 1,
					Multiplier_Factor = 1,
					Status = "AVAILABLE"
				},
				 new Event
				{
					Title = "Event 6",
					StartTime = DateTime.Now.AddMonths(1),
					EndTime = DateTime.Now.AddMonths(1).AddHours(2),
					Description = "Description about the event",
					EventCategoryId = 1,
					Multiplier_Factor = 1,
					Status = "AVAILABLE"
				}
			};
			await context.Event.AddRangeAsync(events);
			await context.SaveChangesAsync();
		}
	}
}