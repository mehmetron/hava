using hava.Controllers;
using hava.Data;
using hava.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Testing.JobControllerTests;

public class Post
{
    
    private readonly DbContextOptions<ApplicationDbContext> _dbContextOptions;

    public Post()
    {
        _dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "test")
            .Options;
    }
    
    // [Fact]
    // public async Task PostJob_Returns_OkResult_With_New_Job()
    // {
    //     var jobPost = new JobPost
    //     {
    //         ClientId = 1,
    //         Name = "Test Job"
    //     };
    //
    //     using (var dbContext = new ApplicationDbContext(_dbContextOptions))
    //     {
    //         var jobController = new JobController(dbContext);
    //
    //         var result = await jobController.PostJob(jobPost);
    //
    //         if (result is OkObjectResult okResult1)
    //         {
    //             var job = okResult1.Value as Job;
    //             // var job = result.Value;
    //                 
    //             Assert.Equal(jobPost.ClientId, job.ClientId);
    //             Assert.Equal(jobPost.Name, job.Name);
    //         }
    //             
    //         // Assert
    //         // var okResult = Assert.IsType<ActionResult<Job>>(result);
    //         // var job = Assert.IsType<Job>(okResult.Value);
    //         //
    //         // Assert.Equal(jobPost.ApplicationUserId, job.ApplicationUserId);
    //         // Assert.Equal(jobPost.Name, job.Name);
    //     }
    // }

        
    [Fact]
    public async Task PostJob_Returns_NotFoundResult_When_JobPost_Is_Null()
    {
        JobPost jobPost = null;

        using (var dbContext = new ApplicationDbContext(_dbContextOptions))
        {
            var jobController = new JobController(dbContext);

            var result = await jobController.PostJob(jobPost);

            Assert.IsType<BadRequestResult>(result);
        }
    }
        
        
        
        
    [Theory]
    [InlineData(1, "some-name")]
    public async Task PostJob_Returns_NoContentResult_With_Valid_Input(int clientId, string name)
    {
        var jobPost = new JobPost
        {
            ClientId = clientId,
            Name = name
        };

        var dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "PostJob_Returns_NoContentResult_With_Valid_Input")
            .Options;

        using (var dbContext = new ApplicationDbContext(dbContextOptions))
        {
            dbContext.Jobs.Add(new Job
                { ClientId = clientId, Name = name });
            dbContext.SaveChanges();
        }

        using (var dbContext = new ApplicationDbContext(dbContextOptions))
        {
            var controller = new JobController(dbContext);

            var result = await controller.PostJob(jobPost);

            Assert.IsType<NoContentResult>(result);
        }
    }
}