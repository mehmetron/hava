using hava.Controllers;
using hava.Data;
using hava.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Testing.ClientControllerTests;

public class Put
{
    private readonly DbContextOptions<ApplicationDbContext> _options = new DbContextOptionsBuilder<ApplicationDbContext>()
        .UseInMemoryDatabase(databaseName: "PutClientTestDB")
        .Options;

    [Fact]
    public async Task PutClient_ReturnsBadRequest_WhenClientPutIsNull()
    {
        // Arrange
        using (var context = new ApplicationDbContext(_options))
        {
            var controller = new ClientController(context);

            // Act
            var result = await controller.PutClient(null);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }
    }

    [Fact]
    public async Task PutClient_ReturnsNotFound_WhenClientIdDoesNotExist()
    {
        // Arrange
        using (var context = new ApplicationDbContext(_options))
        {
            var controller = new ClientController(context);

            var clientPut = new ClientPut
            {
                Id = 4,
                Name = "Client 1",
                ContactInformation = "612"
            };

            // Act
            var result = await controller.PutClient(clientPut);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }

    [Fact]
    public async Task PutClient_UpdatesExistingClient()
    {

        int clientId;
        
        using (var context = new ApplicationDbContext(_options))
        {
            var client = new Client
            {
                Name = "Client 1",
                ContactInformation = "612"
            };

            context.Clients.Add(client);
            await context.SaveChangesAsync();

            clientId = client.Id;
        }

        using (var context = new ApplicationDbContext(_options))
        {
            var controller = new ClientController(context);

            var clientPut = new ClientPut
            {
                Id = clientId,
                Name = "Client 2",
                ContactInformation = "612"
            };

            // Act
            var result = await controller.PutClient(clientPut);

            // Assert
            Assert.IsType<NoContentResult>(result);

            var clientInDb = await context.Clients.FindAsync(clientId);
            Assert.NotNull(clientInDb);
            Assert.Equal("Client 2", clientInDb.Name);
            Assert.Equal("612", clientInDb.ContactInformation);
        }
    }
}
