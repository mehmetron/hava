using hava.Models;
using hava.Data;


namespace hava;

public static class DbInitializer
{
    public static void Initialize(ApplicationDbContext context)
    {
        
        if (context.Users.Any())
        {
            return;
        }

        var user1 = new ApplicationUser() { Email = "obama@gmail.com", UserName = "obama", PasswordHash = "ffff" };
        context.Users.Add(user1);
        var user2 = new ApplicationUser() { Email = "grandma@gmail.com", UserName = "grandma", PasswordHash = "ffff" };
        context.Users.Add(user2);
        var user3 = new ApplicationUser() { Email = "chicken@gmail.com", UserName = "chicken", PasswordHash = "ffff" };
        context.Users.Add(user3);
        context.SaveChanges();
        

        
        var client1 = new Client { Name = "sam", ContactInformation = "651" };
        context.Clients.Add(client1);
        var client2 = new Client { Name = "bob", ContactInformation = "651" };
        context.Clients.Add(client2);
        context.SaveChanges();
        
        
        var job1 = new Job { Name = "Fox", ClientId = client1.Id };
        context.Jobs.Add(job1);
        var job2 = new Job { Name = "prior", ClientId = client1.Id };
        context.Jobs.Add(job2);
        var job3 = new Job { Name = "roseville", ClientId = client2.Id };
        context.Jobs.Add(job3);
        context.SaveChanges();
        
        var userJob1 = new UserJob { ApplicationUserId = user1.Id, JobId = job1.Id };
        context.UserJobs.Add(userJob1);
        var userJob2 = new UserJob { ApplicationUserId = user2.Id, JobId = job2.Id };
        context.UserJobs.Add(userJob2);
        var userJob3 = new UserJob { ApplicationUserId = user3.Id, JobId = job3.Id };
        context.UserJobs.Add(userJob3);
        context.SaveChanges();

    }
}