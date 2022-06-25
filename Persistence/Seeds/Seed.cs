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
            System.Console.WriteLine("----------------- Seed Confirmation ----------------- \n\n\n Are you sure you want to generate the Seed Data? \n\n\n----------------- Seed Confirmation -----------------");
            System.Console.Write("\n Input [y] to confirm: ");
            String seedConfirm =  Console.ReadLine();

			if (seedConfirm != "y") return;
            try{
            System.Console.WriteLine("Seeding...");
            string[] userEmail = new string[1];
            userEmail[0] = "longnnhse150890@fpt.edu.vn";

            string[] eventTitle = new string[2];
            eventTitle[0] = "Talkshow CHAM SACH";

            string[] eventLocation = new string[2];
            eventLocation[0] = "Thư viện Đại học FPT TP.HCM";

            string[] eventDes = new string[2];
            eventDes[0] = "Vào thứ Sáu (10.06.2022) tới đây, Ban Công tác Học đường FPT Edu kết hợp cùng Nhà xuất bản Phụ Nữ sẽ tổ chức một buổi trưng bày các tựa sách hay tại Thư viện Đại học FPT TP.HCM. Trong khuôn khổ chương trình, talkshow “Đọc thế nào?” với sự xuất hiện của hai tác giả: Nguyễn Quốc Vương và Hoàng Anh Đức cũng sẽ được diễn ra với mục đích tạo ra không gian chia sẻ và trao đổi giữa những độc giả yêu sách. \nBên cạnh đó, FPT Edu Experience Space cũng gửi tặng đến các bạn sinh viên Đại học FPT TP.HCM 12 quyển sách thú vị đến từ hai diễn giả của chương trình. Để nhận sách, mời bạn đọc thêm tại bài viết này. \nThư viện FPTU HCM rất mong được chào đón bạn tại Trưng bày và Talkshow Chạm Sách II: Đọc Thế Nào?. Hãy đến và cùng trò chuyện như những tâm hồn đồng điệu nhé!";

            string[] postTitle = new string[3];
            postTitle[0] = "Ra mắt sách nói danh tác hơn trăm năm tuổi 'Người Dublin'";
            postTitle[1] = "Ra mắt bộ tác phẩm 'Việt kiệu thư'";

            string[] postContent = new string[3];
            postContent[0] = "Ireland đã có những đóng góp to lớn cho nền văn học thế giới với 4 giải Nobel Văn chương đã được trao cho các tác gia William Butler Yeats, Samuel Beckett, George Bernard và Seamus Heaney. Quốc gia cũng có nhiều nhà văn nữ nổi tiếng như Anne Enright, Edna O’Brien và Anna Burns - người đoạt giải Man Booker 2018 cho cuốn tiểu thuyết Người giao sữa.";
            postContent[1] = "Bộ sách được đánh giá là một sử liệu quan trọng hàng đầu viết về lịch sử Việt Nam, phản ánh rõ nét tư tưởng, cách nhìn của giới trí thức Trung Quốc về Việt Nam.";


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
				await userManager.AddToRoleAsync(user, "Admin");
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
                    Location = eventLocation[0],
                    Description = eventDes[0],
                    EventCategoryId = context.EventCategories.FirstOrDefault(x => x.Name == "General").Id,
                    MultiplierFactor = 1,
                    Status = StatusEnum.Available,
                    State = EventStateEnum.Publish
                },
                // new Event
                // {
                //     Title = eventTitle[1],
                //     StartTime = DateTime.Now.AddMonths(1),
                //     EndTime = DateTime.Now.AddMonths(1).AddHours(2),
                //     Description = "Description about the event here",
                //     EventCategoryId = 1,
                //     MultiplierFactor = 1,
                //     Status = StatusEnum.Available
                // }
            };
            
            await context.Events.AddRangeAsync(events);
            await context.SaveChangesAsync();

            // ---------------- Post Seed ---------------------- 

            var posts = new List<Post>
            {

                new Post
                {
                    Title = postTitle[0],
                    Content =  postContent[0],
                    Status = StatusEnum.Available,
                    UserId = userManager.Users.FirstOrDefault(x => x.Email == userEmail[0]).Id
                },
                new Post
                {
                    Title = postTitle[1],
                    Content =  postContent[1],
                    Status = StatusEnum.Available,
                    UserId = userManager.Users.FirstOrDefault(x => x.Email == userEmail[0]).Id
                },
                // new Post
                // {
                //     Title = postTitle[2],
                //     Content = "Content of the post here",
                //     Status = StatusEnum.Available,
                //     UserId = userManager.Users.FirstOrDefault(x => x.Email == userEmail[0]).Id
                // },
            };


            await context.Posts.AddRangeAsync(posts);
            await context.SaveChangesAsync();

            // ---------------- Comment Seed ---------------------- 

            var comments = new List<Comment>{
                new Comment {
                    Body = "I love this event!",
                    PostId = context.Posts.FirstOrDefault(x => x.Title == postTitle[0]).Id,
                    UserId = userManager.Users.FirstOrDefault(x => x.Email == userEmail[0]).Id
                },
                new Comment {
                    Body = "I love this event!!!!!",
                    PostId = context.Posts.FirstOrDefault(x => x.Title == postTitle[1]).Id,
                    UserId = userManager.Users.FirstOrDefault(x => x.Email == userEmail[0]).Id
                },
                // new Comment {
                //     Body = "I love this event!!!!!!!!!!!!!",
                //     PostId = context.Posts.FirstOrDefault(x => x.Title == postTitle[3]).Id
                // }
            };
            await context.Comments.AddRangeAsync(comments);
            await context.SaveChangesAsync();

            // ---------------- Organizer Seed ---------------------- 

            var organizers = new List<Organizer>{
                new Organizer {
                    Name = "FPT University",
                    Description = "Chính thức thành lập ngày 8/9/2006 theo Quyết định của Thủ tướng Chính phủ, Trường Đại học FPT trở thành trường đại học đầu tiên của Việt Nam do một doanh nghiệp đứng ra thành lập với 100% vốn đầu tư từ Tập đoàn FPT.",
                    Status = StatusEnum.Available
                },
            };
            await context.Organizers.AddRangeAsync(organizers);
            await context.SaveChangesAsync();

            // ---------------- Ticket Seed ---------------------- 

            var tickets = new List<Ticket>{
                new Ticket
				{
					Type = "Casual",
					Name = "Casual",
					Description = "Back Seat Row",
					Cost = 0,
					EventId = context.Events.FirstOrDefault(x => x.Title == eventTitle[0]).Id,
					Quantity = 50
				},
				new Ticket
				{
					Type = "VIP",
					Name = "VIP",
					Description = "Front Seat Row",
					Cost = 0,
					EventId = context.Events.FirstOrDefault(x => x.Title == eventTitle[0]).Id,
					Quantity = 10
				},
            };
            await context.Tickets.AddRangeAsync(tickets);
            await context.SaveChangesAsync();

            // ---------------- EventAgenda Seed ---------------------- 

            var eventAgendas = new List<EventAgenda>{
                new EventAgenda
				{
                    Title = "The Opening",
					Description = "Description For agenda",
					EventId = context.Events.FirstOrDefault(x => x.Title == eventTitle[0]).Id,
                    StartTime = DateTime.Now.AddMonths(1).AddMinutes(15),
                    EndTime = DateTime.Now.AddMonths(1).AddMinutes(45),
                },
                new EventAgenda
				{
                    Title = "The Middle",
					Description = "Description For agenda",
					EventId = context.Events.FirstOrDefault(x => x.Title == eventTitle[0]).Id,
                    StartTime = DateTime.Now.AddMonths(1).AddMinutes(50),
                    EndTime = DateTime.Now.AddMonths(1).AddMinutes(90),
                },
                new EventAgenda
				{
                    Title = "The Ending",
					Description = "Description For agenda",
					EventId = context.Events.FirstOrDefault(x => x.Title == eventTitle[0]).Id,
                    StartTime = DateTime.Now.AddMonths(1).AddMinutes(95),
                    EndTime = DateTime.Now.AddMonths(1).AddMinutes(120),
                },
            };
            await context.EventAgendas.AddRangeAsync(eventAgendas);
            await context.SaveChangesAsync();

             // ---------------- EventUser Seed ---------------------- 

            var eventUsers = new List<EventUser>{
                new EventUser {
                    Status = StatusEnum.Available,
                    Type = EventUserTypeEnum.Creator,
                    EventId = context.Events.FirstOrDefault(x => x.Title == eventTitle[0]).Id,
                    UserId = userManager.Users.FirstOrDefault(x => x.Email == userEmail[0]).Id
                },
            };
            await context.EventUsers.AddRangeAsync(eventUsers);
            await context.SaveChangesAsync();
            
            // ---------------- TicketUser ----------------------

            var ticketUser = new List<TicketUser>{
                new TicketUser {
                    TicketId = context.Tickets.FirstOrDefault(x => x.Event.Title == eventTitle[0] && x.Name == "Casual").Id,
                    UserId = userManager.Users.FirstOrDefault(x => x.Email == userEmail[0]).Id,
                    State = TicketUserStateEnum.Idle
                },
                new TicketUser {
                    TicketId = context.Tickets.FirstOrDefault(x => x.Event.Title == eventTitle[0] && x.Name == "VIP").Id,
                    UserId = userManager.Users.FirstOrDefault(x => x.Email == userEmail[0]).Id,
                    State = TicketUserStateEnum.Idle
                },
            };
            await context.TicketUsers.AddRangeAsync(ticketUser);
            await context.SaveChangesAsync();

            // ---------------- EventOrganizer ----------------------

            var eventOrganizers = new List<EventOrganizer>{
                new EventOrganizer {

                    EventId = context.Events.FirstOrDefault(x => x.Title == eventTitle[0]).Id,
                    OrganizerId = context.Organizers.FirstOrDefault(x => x.Name == "FPT University").Id,
                }
            };
            await context.EventOrganizers.AddRangeAsync(eventOrganizers);
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

                // context.UserImages.RemoveRange(context.UserImages);
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