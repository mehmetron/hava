using hava.Controllers;
using hava.Data;
using hava.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Testing.JobControllerTests;

public class Get
{
    
    private readonly DbContextOptions<ApplicationDbContext> _dbContextOptions;

    public Get()
    {
        _dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "GetJobTestDB")
            .Options;
    }
    
    [Fact]
    public async Task GetJobs_Returns_UserJobs()
    {
        var clientId = 1;
        var homes = new List<Job>
        {
            new Job { Id = 10, Name = "fox", ClientId = clientId },
            new Job { Id = 20, Name = "bob", ClientId = clientId },
            new Job { Id = 30, Name = "chicken", ClientId = clientId },
        }.AsQueryable();

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "GetJobs_Returns_UserJobs")
            .Options;
        var dbContext = new ApplicationDbContext(options);
        dbContext.Jobs.AddRange(homes);
        await dbContext.SaveChangesAsync();

        var controller = new JobController(dbContext);
        
        var result = await controller.GetClientJobs(clientId);

        var JobsResult = result.Value.ToList();
        Assert.Equal(3, JobsResult.Count);
        Assert.Contains(JobsResult, h => h.Id == 10);
        Assert.Contains(JobsResult, h => h.Id == 30);
    }

    [Theory]
    [InlineData(1, "Job 1", 1)]
    public async Task GetJob_ReturnsJobGet_WhenJobFound(int id, string name, int clientId)
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "GetJob_ReturnsJobGet_WhenJobFound")
            .Options;
            
        using (var context = new ApplicationDbContext(options))
        {
            context.Jobs.Add(new Job { Id = id, Name = name, ClientId  = clientId });
            await context.SaveChangesAsync();
        }

        using (var context = new ApplicationDbContext(options))
        {
            var controller = new JobController(context);

            var result = await controller.GetJob(1);

            Assert.IsType<Job>(result.Value);
            Assert.Equal(id, result.Value.Id);
            Assert.Equal(name, result.Value.Name);
            Assert.Equal(clientId, result.Value.ClientId);
        }
    }
        
        
    [Fact]
    public async Task GetJob_ReturnsNotFound_WhenJobNotFound()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "GetJob_ReturnsNotFound_WhenJobNotFound")
            .Options;
            
        var id = 1;
        var name = "Job 1";
        var clientId = 1;
            
        using (var context = new ApplicationDbContext(options))
        {
            context.Jobs.Add(new Job { Id = id, Name = name, ClientId  = clientId });
            await context.SaveChangesAsync();
        }
        using (var context = new ApplicationDbContext(options))
        {
            var controller = new JobController(context);

            var result = await controller.GetJob(2);

            Assert.IsType<NotFoundResult>(result.Result);
        }
    }
}