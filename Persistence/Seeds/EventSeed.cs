using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Domain.Enums;

namespace Persistence
{
	public class EventSeed
	{
		public static async Task SeedData(DataContext context)
		{
			if (context.Events.Any()) return;

			var events = new List<Event>
			{
				new Event
				{
					Title = "Event 1",
					StartTime = DateTime.Now.AddMonths(1),
					EndTime = DateTime.Now.AddMonths(1).AddHours(2),
					Description = "Description about the event",
					EventCategoryId = 1,
					MultiplierFactor = 1,
					Status = StatusEnum.Available
				},
				 new Event
				{
					Title = "Event 2",
					StartTime = DateTime.Now.AddMonths(1),
					EndTime = DateTime.Now.AddMonths(1).AddHours(2),
					Description = "Description about the event",
					EventCategoryId = 1,
					MultiplierFactor = 1,
					Status = StatusEnum.Available
				},
				 new Event
				{
					Title = "Event 3",
					StartTime = DateTime.Now.AddMonths(1),
					EndTime = DateTime.Now.AddMonths(1).AddHours(2),
					Description = "Description about the event",
					EventCategoryId = 1,
					MultiplierFactor = 1,
					Status = StatusEnum.Available
				},
				 new Event
				{
					Title = "Event 4",
					StartTime = DateTime.Now.AddMonths(1),
					EndTime = DateTime.Now.AddMonths(1).AddHours(2),
					Description = "Description about the event",
					EventCategoryId = 1,
					MultiplierFactor = 1,
					Status = StatusEnum.Available
				},
				 new Event
				{
					Title = "Event 5",
					StartTime = DateTime.Now.AddMonths(1),
					EndTime = DateTime.Now.AddMonths(1).AddHours(2),
					Description = "Description about the event",
					EventCategoryId = 1,
					MultiplierFactor = 1,
					Status = StatusEnum.Available
				},
				 new Event
				{
					Title = "Event 6",
					StartTime = DateTime.Now.AddMonths(1),
					EndTime = DateTime.Now.AddMonths(1).AddHours(2),
					Description = "Description about the event",
					EventCategoryId = 1,
					MultiplierFactor = 1,
					Status = StatusEnum.Available
				}
			};
			await context.Events.AddRangeAsync(events);
			await context.SaveChangesAsync();
		}
	}
}