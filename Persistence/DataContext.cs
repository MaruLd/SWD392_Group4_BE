using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
	public class DataContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
	{
		public DataContext(DbContextOptions options) :
			base(options)
		{

		}


		public DbSet<User> Users { get; set; }

		public DbSet<Event> Events { get; set; }

		public DbSet<EventAgenda> EventAgendas { get; set; }

		public DbSet<Organizer> Organizers { get; set; }

		public DbSet<Post> Posts { get; set; }

		public DbSet<Ticket> Tickets { get; set; }

		public DbSet<EventUser> EventUsers { get; set; }

		public DbSet<EventCategory> EventCategories { get; set; }

		public DbSet<Comment> Comments { get; set; }

		// public DbSet<EventTicket> EventTickets { get; set; }

		// protected override void OnModelCreating(ModelBuilder builder)
		// {
		//     base.OnModelCreating(builder);

		//     builder.Entity<EventTicket>(x => x.HasKey(et => new { et.TicketId, et.EventId }));

		//     builder.Entity<EventTicket>().HasOne(t => t.Ticket).WithMany(e => e.EventTickets).HasForeignKey(et => et.TicketId);
		//     builder.Entity<EventTicket>().HasOne(t => t.Event).WithMany(e => e.EventTickets).HasForeignKey(et => et.EventId);
		// }
	}
}
