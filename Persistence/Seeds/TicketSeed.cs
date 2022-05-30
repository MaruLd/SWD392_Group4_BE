using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;

namespace Persistence
{
	public class TicketSeed
	{
		public static async Task SeedData(DataContext context)
		{
			if (context.Ticket.Any()) return;

			var tickets = new List<Ticket>
			{
				new Ticket
				{
					Type = "Casual",
					Name = "Ticket 1",
					Description = "Description For Ticket",
					Cost = 0,
					Quantity = 50
				},
				new Ticket
				{
					Type = "Hardcore",
										Name = "Ticket 2",
					Description = "Description For Ticket",
					Cost = 0,
					Quantity = 10
				},
				new Ticket
				{
					Type = "Casual",
										Name = "Ticket 3",
					Description = "Description For Ticket",
					Cost = 0,
					Quantity = 50
				},
				new Ticket
				{
					Type = "Hardcore",
										Name = "Ticket 4",
					Description = "Description For Ticket",
					Cost = 0,
					Quantity = 10
				},
				new Ticket
				{
					Type = "Casual",
										Name = "Ticket 5",
					Description = "Description For Ticket",
					Cost = 0,
					Quantity = 50
				},
				new Ticket
				{
					Type = "Vip",
										Name = "Ticket 6",
					Description = "Description For Ticket",
					Cost = 0,
					Quantity = 10
				},
				new Ticket
				{
					Type = "Casual",
										Name = "Ticket 7",
					Description = "Description For Ticket",
					Cost = 0,
					Quantity = 50
				},

				new Ticket
				{
					Type = "Casual",
										Name = "Ticket 8",
					Description = "Description For Ticket",
					Cost = 0,
					Quantity = 50
				},

				new Ticket
				{
					Type = "Casual",
					Name = "Ticket 9",
					Description = "Description For Ticket",
					Cost = 0,
					Quantity = 50
				},
			};

			await context.Ticket.AddRangeAsync(tickets);
			await context.SaveChangesAsync();
		}
	}
}