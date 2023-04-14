using hava.Controllers;
using hava.Data;
using hava.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Testing.ClientControllerTests;

public class Delete
{
    private readonly DbContextOptions<ApplicationDbContext> _options = new DbContextOptionsBuilder<ApplicationDbContext>()
        .UseInMemoryDatabase(databaseName: "DeleteClientTestDB")
        .Options;

    
    // [Fact]
    // public async Task DeleteClient_ReturnsBadRequest_WhenClientIsNull()
    // {
    //     // Arrange
    //     using (var context = new ApplicationDbContext(_options))
    //     {
    //         var controller = new ClientController(context);
    //
    //         // Act
    //         var result = await controller.DeleteClient(null);
    //
    //         // Assert
    //         Assert.IsType<BadRequestResult>(result.Result);
    //     }
    // }
    
    [Fact]
    public async Task DeleteClient_ReturnsNotFound_WhenClientDoesNotExist()
    {
        // Arrange
        using (var context = new ApplicationDbContext(_options))
        {
            var controller = new ClientController(context);

            // Act
            var result = await controller.DeleteClient(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }

    [Fact]
    public async Task DeleteClient_RemovesClientFromDatabase()
    {
        // Arrange
        using (var context = new ApplicationDbContext(_options))
        {
            context.Clients.Add(new Client
            {
                Id = 1, 
                Name = "Client 1",
                ContactInformation = "651"
            });
            context.SaveChanges();

            var controller = new ClientController(context);

            // Act
            var result = await controller.DeleteClient(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
            Assert.Null(context.Clients.Find(1));
        }
    }
}
