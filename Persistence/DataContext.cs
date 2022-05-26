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
        public DbSet<Event> Event { get; set; }
        public DbSet<EventAgenda> EventAgenda { get; set; }
        public DbSet<Organizer> Organizer { get; set; }
        public DbSet<Post> Post { get; set; }
        public DbSet<Ticket> Ticket { get; set; }
        public DbSet<Participant> Participant { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Comment> Comment { get; set; }
    }
}