using hava.Controllers;
using hava.Data;
using hava.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Testing.ClientControllerTests;

public class Post
{
    private readonly DbContextOptions<ApplicationDbContext> _options = new DbContextOptionsBuilder<ApplicationDbContext>()
        .UseInMemoryDatabase(databaseName: "PostClientTestDB")
        .Options;

    [Fact]
    public async Task PostClient_ReturnsBadRequest_WhenClientPostIsNull()
    {
        using (var context = new ApplicationDbContext(_options))
        {
            var controller = new ClientController(context);

            var result = await controller.PostClient(null);

            Assert.IsType<BadRequestResult>(result);
        }
    }

    [Fact]
    public async Task PostClient_CreatesNewClient()
    {
        using (var context = new ApplicationDbContext(_options))
        {
            var controller = new ClientController(context);

            var clientPost = new ClientPost
            {
                Name = "client 1",
                ContactInformation = "651"
            };

            var result = await controller.PostClient(clientPost);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var client = Assert.IsType<Client>(createdResult.Value);

            Assert.Equal("client 1", client.Name);
            Assert.Equal("651", client.ContactInformation);

            // var clientInDb = await context.Clients.FindAsync(client.Id);
            // Assert.NotNull(clientInDb);
            Assert.Equal("client 1", client.Name);
            Assert.Equal("651", client.ContactInformation);
        }
    }
}
