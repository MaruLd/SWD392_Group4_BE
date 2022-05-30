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

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			builder.Entity<EventTicket>()
			.HasKey(e => new { e.EventId, e.TicketId });
		}

		public DbSet<User> User { get; set; }

		public DbSet<Event> Event { get; set; }

		public DbSet<EventAgenda> EventAgenda { get; set; }

		public DbSet<Organizer> Organizer { get; set; }

		public DbSet<Post> Post { get; set; }

		public DbSet<Ticket> Ticket { get; set; }

		public DbSet<Participant> Participant { get; set; }

		public DbSet<EventCategory> EventCategory { get; set; }

		public DbSet<Comment> Comment { get; set; }

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
