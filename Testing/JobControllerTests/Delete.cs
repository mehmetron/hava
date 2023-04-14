using hava.Controllers;
using hava.Data;
using hava.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Testing.JobControllerTests;

public class Delete
{
    
        private readonly DbContextOptions<ApplicationDbContext> _dbContextOptions;

        public Delete()
        {
            // Set up an in-memory database for testing
            _dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "DeleteJobTestDB")
                .Options;
        }
        
        
        [Theory]
        [InlineData(1)]
        public async Task DeleteJob_ReturnsNotFound_WhenJobNotFound(int id)
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "DeleteJob_ReturnsNotFound_WhenJobNotFound")
                .Options;
            using (var context = new ApplicationDbContext(options))
            {
                var controller = new JobController(context);

                // Act
                var result = await controller.DeleteJob(id);

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        } 
        
        [Theory]
        [InlineData(1, "Job 1", 1)]
        public async Task DeleteJob_RemovesJobAndReturnsNoContent_WhenJobFound(int id, string name, int clientId)
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "DeleteJob_RemovesJobAndReturnsNoContent_WhenJobFound")
                .Options;
            
            // var date = DateTime.Now.ToString();
            using (var context = new ApplicationDbContext(options))
            {
                context.Jobs.Add(new Job { Id = id, Name = name, ClientId = clientId });
                await context.SaveChangesAsync();
            }
            using (var context = new ApplicationDbContext(options))
            {
                var controller = new JobController(context);

                // Act
                var result = await controller.DeleteJob(1);

                // Assert
                Assert.IsType<NoContentResult>(result);
                Assert.Null(await context.Jobs.FindAsync(1));
            }
        }
}