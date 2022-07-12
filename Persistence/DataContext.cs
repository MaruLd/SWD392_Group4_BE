using System.Data;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Persistence
{
	public class DataContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
	{
		public DataContext(DbContextOptions options) : base(options)
		{

		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			modelBuilder.Entity<Comment>()
			  .HasMany(e => e.CommentLikes)
			  .WithOne(e => e.Comment)
			  .OnDelete(DeleteBehavior.Restrict);

			/*
			var Comment = db.Set<Comment>().Include(e => e.CommentLikes).Single(e => e.Id == id);
			db.RemoveRange(Comment.CommentLikes);
			db.Remove(productType);
			db.SaveChanges();
			*/
		}

		//protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		//{
		//    if (!optionsBuilder.IsConfigured)
		//    {
		//        optionsBuilder.UseSqlServer("Server=evsmart-database.cmiv12nvywqh.ap-southeast-1.rds.amazonaws.com,1433;Initial Catalog=SmartEvent;Persist Security Info=False;User ID=admin1;Password=Kindly?Estranged?Overrule4?Bonding?Facebook;");
		//    }
		//}

		public DbSet<User> Users { get; set; }

		public DbSet<Event> Events { get; set; }

		public DbSet<EventAgenda> EventAgendas { get; set; }

		public DbSet<Organizer> Organizers { get; set; }

		public DbSet<Post> Posts { get; set; }

		public DbSet<Ticket> Tickets { get; set; }

		public DbSet<TicketUser> TicketUsers { get; set; }

		public DbSet<EventUser> EventUsers { get; set; }

		public DbSet<EventCategory> EventCategories { get; set; }

		public DbSet<Comment> Comments { get; set; }

		public DbSet<CommentLike> CommentLikes { get; set; }

		public DbSet<EventOrganizer> EventOrganizers { get; set; }

		public DbSet<UserImage> UserImages { get; set; }

		public DbSet<UserFCMToken> UserFCMTokens { get; set; }

		public DbSet<EventCode> EventCodes { get; set; }
	}
}
