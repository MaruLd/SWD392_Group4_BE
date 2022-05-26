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
                    Cost = 0,
                    EventId = 2,
                    Quantity = 50
                },
                new Ticket
                {
                    Type = "Hardcore",
                    Cost = 0,
                    EventId = 2,
                    Quantity = 10
                },
                new Ticket
                {
                    Type = "Casual",
                    Cost = 0,
                    EventId = 3,
                    Quantity = 50
                },
                new Ticket
                {
                    Type = "Hardcore",
                    Cost = 0,
                    EventId = 3,
                    Quantity = 10
                },
                new Ticket
                {
                    Type = "Casual",
                    Cost = 0,
                    EventId = 4,
                    Quantity = 50
                },
                new Ticket
                {
                    Type = "Vip",
                    Cost = 0,
                    EventId = 4,
                    Quantity = 10
                },
                new Ticket
                {
                    Type = "Casual",
                    Cost = 0,
                    EventId = 5,
                    Quantity = 50
                },
                
                new Ticket
                {
                    Type = "Casual",
                    Cost = 0,
                    EventId = 6,
                    Quantity = 50
                },
                
                new Ticket
                {
                    Type = "Casual",
                    Cost = 0,
                    EventId = 7,
                    Quantity = 50
                },
                
            };
            await context.Ticket.AddRangeAsync(tickets);
            await context.SaveChangesAsync();
        }
    }
}