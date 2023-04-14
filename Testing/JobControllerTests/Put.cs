using hava.Controllers;
using hava.Data;
using hava.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Testing.JobControllerTests;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Xunit;

public class Put
{
        private readonly DbContextOptions<ApplicationDbContext> _dbContextOptions;

        public Put()
        {
            _dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "PutControllerTestDB")
                .Options;
        }
        
    [Theory]
    [InlineData(1, 1, "some-name")]
    public async Task PutJob_Returns_NoContentResult_With_Valid_Input(int id, int clientId, string name)
    {
        var jobPut = new JobPut
        {
            Id = id,
            ClientId = clientId,
            Name = name
        };

        var dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "PutJob_Returns_NoContentResult_With_Valid_Input")
            .Options;

        using (var dbContext = new ApplicationDbContext(dbContextOptions))
        {
            dbContext.Jobs.Add(new Job
                { Id = id, ClientId = clientId, Name = name });
            dbContext.SaveChanges();
        }

        using (var dbContext = new ApplicationDbContext(dbContextOptions))
        {
            var controller = new JobController(dbContext);

            var result = await controller.PutJob(jobPut);

            Assert.IsType<NoContentResult>(result);
        }
    }
    
    [Fact]
    public async Task PutJob_Returns_BadRequestResult_With_Null_Input()
    {
        JobPut jobPut = null;

        var dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        using (var dbContext = new ApplicationDbContext(dbContextOptions))
        {
            var controller = new JobController(dbContext);

            var result = await controller.PutJob(jobPut);

            Assert.IsType<BadRequestResult>(result);
        }
    }

    [Theory]
    [InlineData(1, 1, "some-name", 2)]
    public async Task PutJob_Returns_NotFoundResult_With_Invalid_Input(int id, int clientId, string name, int nonExistantId)
    {
        var jobPut = new JobPut
        {
            Id = nonExistantId,
            ClientId = clientId,
            Name = name
        };

        var dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        using (var dbContext = new ApplicationDbContext(dbContextOptions))
        {
            dbContext.Jobs.Add(new Job { Id = id, ClientId = clientId, Name = name });
            dbContext.SaveChanges();

            var controller = new JobController(dbContext);

            var result = await controller.PutJob(jobPut);

            Assert.IsType<NotFoundResult>(result);
        }
    }
}
