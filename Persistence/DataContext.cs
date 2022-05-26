using Domain;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventAgenda> EventAgendas { get; set; }
        public DbSet<Organizer> Organizers { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Participant> Participants { get; set; }
        public DbSet<EventCategory> EventCategories { get; set; }
        public DbSet<Comment> Comments { get; set; }
    }
}