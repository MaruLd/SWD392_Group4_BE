using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
	public class TicketSeed
	{
		public static async Task SeedData(DataContext context)
		{
			if (context.Tickets.Any()) return;
			var randomEvent = await context.Events.FirstOrDefaultAsync();
			var tickets = new List<Ticket>
			{
				new Ticket
				{
					Type = "Casual",
					Name = "Ticket 1",
					Description = "Description For Ticket",
					Cost = 0,
					EventId = randomEvent.Id,
					Quantity = 50
				},
				new Ticket
				{
					Type = "Hardcore",
										Name = "Ticket 2",
					Description = "Description For Ticket",
					Cost = 0,
					EventId = randomEvent.Id,
					Quantity = 10
				},
				new Ticket
				{
					Type = "Casual",
										Name = "Ticket 3",
					Description = "Description For Ticket",
					Cost = 0,
					EventId = randomEvent.Id,
					Quantity = 50
				},
				new Ticket
				{
					Type = "Hardcore",
										Name = "Ticket 4",
					Description = "Description For Ticket",
					Cost = 0,
					EventId = randomEvent.Id,
					Quantity = 10
				},
				new Ticket
				{
					Type = "Casual",
										Name = "Ticket 5",
					Description = "Description For Ticket",
					Cost = 0,
					EventId = randomEvent.Id,
					Quantity = 50
				},
				new Ticket
				{
					Type = "Vip",
										Name = "Ticket 6",
					Description = "Description For Ticket",
					Cost = 0,
					EventId = randomEvent.Id,
					Quantity = 10
				},
				new Ticket
				{
					Type = "Casual",
										Name = "Ticket 7",
					Description = "Description For Ticket",
					Cost = 0,
					EventId = randomEvent.Id,
					Quantity = 50
				},

				new Ticket
				{
					Type = "Casual",
										Name = "Ticket 8",
					Description = "Description For Ticket",
					Cost = 0,
					EventId = randomEvent.Id,
					Quantity = 50
				},

				new Ticket
				{
					Type = "Casual",
					Name = "Ticket 9",
					Description = "Description For Ticket",
					Cost = 0,
					EventId = randomEvent.Id,
					Quantity = 50
				},
			};

			await context.Tickets.AddRangeAsync(tickets);
			await context.SaveChangesAsync();
		}
	}
}