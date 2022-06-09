using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace Persistence
{
    public class Seed
    {
        public static async Task SeedData(DataContext context, UserManager<User> userManager)
        {
            System.Console.WriteLine("----------------- Seed Confirmation ----------------- \n\n\n Are you sure you want to generate the Seed Data? \n\n\n ----------------- Seed Confirmation -----------------");
            System.Console.Write("\n Input [y] to confirm: ");
            String seedConfirm =  Console.ReadLine();
            
			if (seedConfirm != "y") return;
            try{
            System.Console.WriteLine("Seeding...");
            string[] userEmail = new string[1];
            userEmail[0] = "longnnhse150890@fpt.edu.vn";

            string[] eventTitle = new string[2];
            eventTitle[0] = "The Event 1 Title Go Here";
            eventTitle[1] = "The Event 2 Title Go Here";

            string[] postTitle = new string[3];
            postTitle[0] = "The Post 1 Title of Event 1";
            postTitle[1] = "The Post 2 Title of Event 1";
            postTitle[2] = "The Post 1 Title of Event 2";

            // ---------------- User Seed ----------------------

            var users = new List<User>
            {
				new User{
					DisplayName = "Admin",
					Email = userEmail[0],
					UserName = userEmail[0]
				}
            };
			foreach(var user in users){
				await userManager.CreateAsync(user);
			}
            
            // ---------------- EventCategory Seed ----------------------

            await context.EventCategories.AddRangeAsync(new EventCategory() { Name = "General" });
			await context.SaveChangesAsync();

            // ---------------- Event Seed ----------------------

            var events = new List<Event>
            {
                new Event
                {
                    Title = eventTitle[0],
                    StartTime = DateTime.Now.AddMonths(1),
                    EndTime = DateTime.Now.AddMonths(1).AddHours(2),
                    Description = "Description about the event here",
                    EventCategoryId = 1,
                    MultiplierFactor = 1,
                    Status = StatusEnum.Available
                },
                new Event
                {
                    Title = eventTitle[1],
                    StartTime = DateTime.Now.AddMonths(1),
                    EndTime = DateTime.Now.AddMonths(1).AddHours(2),
                    Description = "Description about the event here",
                    EventCategoryId = 1,
                    MultiplierFactor = 1,
                    Status = StatusEnum.Available
                }
            };

            await context.Events.AddRangeAsync(events);
            await context.SaveChangesAsync();

            // ---------------- Post Seed ---------------------- 

            var posts = new List<Post>
            {

                new Post
                {
                    Title = postTitle[0],
                    Content = "Content of the post here",
                    Status = StatusEnum.Available,
                    UserId = userManager.Users.FirstOrDefault(x => x.Email == userEmail[0]).Id
                },
                new Post
                {
                    Title = postTitle[1],
                    Content = "Content of the post here",
                    Status = StatusEnum.Available,
                    UserId = userManager.Users.FirstOrDefault(x => x.Email == userEmail[0]).Id
                },
                new Post
                {
                    Title = postTitle[2],
                    Content = "Content of the post here",
                    Status = StatusEnum.Available,
                    UserId = userManager.Users.FirstOrDefault(x => x.Email == userEmail[0]).Id
                },
            };


            await context.Posts.AddRangeAsync(posts);
            await context.SaveChangesAsync();

            // ---------------- Comment Seed ---------------------- 

            var comments = new List<Comment>{
                new Comment {
                    Body = "I love this event!",
                    PostId = context.Posts.FirstOrDefault(x => x.Title == postTitle[0]).Id,

                },
                new Comment {
                    Body = "I love this event!!!!!",
                    PostId = context.Posts.FirstOrDefault(x => x.Title == postTitle[1]).Id
                },
                new Comment {
                    Body = "I love this event!!!!!!!!!!!!!",
                    PostId = context.Posts.FirstOrDefault(x => x.Title == postTitle[3]).Id
                }
            };
            await context.Comments.AddRangeAsync(comments);
            await context.SaveChangesAsync();

            }catch(Exception ex){
                System.Console.WriteLine(ex);
            }
        }

        public static async Task ClearSeedData(DataContext context, UserManager<User> userManager)
        {
            try{
               
                foreach(var user in userManager.Users){
                    userManager.DeleteAsync(user);
                }
                context.EventCategories.RemoveRange(context.EventCategories);
                context.Events.RemoveRange(context.Events);
                context.Posts.RemoveRange(context.Posts);
                context.Comments.RemoveRange(context.Comments);
                 context.EventUsers.RemoveRange(context.EventUsers);
                context.Tickets.RemoveRange(context.Tickets);
                context.EventAgendas.RemoveRange(context.EventAgendas);
                context.TicketUsers.RemoveRange(context.TicketUsers);
                context.EventOrganizers.RemoveRange(context.EventOrganizers);
                context.Organizers.RemoveRange(context.Organizers);
                
                context.SaveChangesAsync();
            }catch(Exception ex){
                System.Console.WriteLine(ex);
            }
        }
    }
}