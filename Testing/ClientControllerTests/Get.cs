using hava.Controllers;
using hava.Data;
using hava.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Testing.ClientControllerTests;

public class Get
{
    
    private readonly DbContextOptions<ApplicationDbContext> _dbContextOptions;

    public Get()
    {
        // Set up an in-memory database for testing
        _dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "GetClientTestDB")
            .Options;
    }
    
    [Fact]
    public async Task GetJobs_Returns_ClientJobs()
    {
            
        var clientId = 1;
        var clients = new List<Client>
        {
            new Client { Id = 1, Name = "Drake", ContactInformation = "f" },
            new Client { Id = 2, Name = "liljohn", ContactInformation = "e" },
            new Client { Id = 3, Name = "eminem", ContactInformation = "i" },
        }.AsQueryable();
        
        var jobs = new List<Job>
        {
            new Job { Id = 1, Name = "Drake", ClientId = clientId },
            new Job { Id = 2, Name = "Drake", ClientId = clientId },
            new Job { Id = 3, Name = "Drake", ClientId = clientId },
        }.AsQueryable();

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "GetJobs_Returns_ClientJobs")
            .Options;
            
        var dbContext = new ApplicationDbContext(options);
        dbContext.Clients.AddRange(clients);
        dbContext.Jobs.AddRange(jobs);
        await dbContext.SaveChangesAsync();

        var controller = new ClientController(dbContext);

        var result = await controller.GetJobs(clientId);

        var homesResult = result.Value.ToList();
        Assert.Equal(3, homesResult.Count);
        Assert.Contains(homesResult, h => h.Id == 1);
        Assert.Contains(homesResult, h => h.Id == 3);
    }

    [Theory]
    [InlineData(11, "Client 1", "651")]
    public async Task GetClient_ReturnsClientGet_WhenClientFound(int id, string name, string contactInformation)
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "GetClient_ReturnsClientGet_WhenClientFound")
            .Options;
            
        using (var context = new ApplicationDbContext(options))
        {
            context.Clients.Add(new Client { Id = id, Name = name, ContactInformation = contactInformation });
            await context.SaveChangesAsync();
        }

        using (var context = new ApplicationDbContext(options))
        {
            var controller = new ClientController(context);

            // Act
            var result = await controller.GetClient(id);

            // Assert
            Assert.IsType<Client>(result.Value);
            Assert.Equal(id, result.Value.Id);
            Assert.Equal(name, result.Value.Name);
            Assert.Equal(contactInformation, result.Value.ContactInformation);
        }
    }
        
        
    [Theory]
    [InlineData(20, "Home 1", "651")]
    public async Task GetClient_ReturnsNotFound_WhenClientNotFound(int id, string name, string contactInformation)
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "GetClient_ReturnsNotFound_WhenClientNotFound")
            .Options;
            
        var client = new Client { Id = id, Name = name, ContactInformation = contactInformation};
        using (var context = new ApplicationDbContext(options))
        {
            context.Clients.Add(client);
            await context.SaveChangesAsync();
        }
        using (var context = new ApplicationDbContext(options))
        {
            var controller = new ClientController(context);

            // Act
            var result = await controller.GetClient(2);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }
    }
}