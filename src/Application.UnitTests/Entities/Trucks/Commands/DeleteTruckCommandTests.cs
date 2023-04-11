using Application.Entities.Trucks.Commands;
using Application.Entities.Trucks.Queries;
using Application.Exception;

namespace Application.UnitTests.Entities.Trucks.Commands;
public class DeleteTruckCommandTests
{
    private readonly Mock<IDatabaseManager<Truck>> _mockDatabaseManager;
    private readonly DeleteTruckCommandHandler _handler;

    public DeleteTruckCommandTests()
    {
        _mockDatabaseManager  = new Mock<IDatabaseManager<Truck>>();
        _handler = new DeleteTruckCommandHandler(_mockDatabaseManager.Object);
    }

    [Fact]
    public async Task Handle_ShouldDeleteTruck_WhenTruckExists()
    {
        // Arrange
        var truckId = 1;
        Truck existingTruck = new Truck { Id = truckId, Name = "TruckName", Paint = Paint.Black, Items = new List<TruckPart>() };
        _mockDatabaseManager.Setup(m => m.ApplicationRepository.GetByIdAsync(truckId, default))
                           .ReturnsAsync(existingTruck);
        var command = new DeleteTruckCommand { TruckId = truckId };

        // Act
        await _handler.Handle(command, default);

        // Assert
        _mockDatabaseManager.Verify(m => m.ApplicationRepository.DeleteAsync(existingTruck, default), Times.Once);
        _mockDatabaseManager.Verify(m => m.ApplicationRepository.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenTruckDoesNotExist()
    {
        // Arrange
        var truckId = 1;
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        _mockDatabaseManager.Setup(m => m.ApplicationRepository.GetByIdAsync(truckId, default))
                           .ReturnsAsync((Truck)null);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
        var command = new DeleteTruckCommand { TruckId = truckId };

        // Act + Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, default));
    }
}