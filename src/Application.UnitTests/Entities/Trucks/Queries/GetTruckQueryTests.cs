using Application.Entities.Trucks.Queries;
using Application.Exception;

namespace Application.UnitTests.Entities.Trucks.Queries;
public class GetTruckQueryTests
{
    private readonly Mock<IDatabaseManager<Truck>> _mockDatabaseManager;
    private readonly Mock<IMapper<TruckDTO, Truck>> _mockMapper;
    private readonly GetTruckQueryHandler _handler;

    public GetTruckQueryTests()
    {
        _mockDatabaseManager = new Mock<IDatabaseManager<Truck>>();
        _mockMapper = new Mock<IMapper<TruckDTO, Truck>>();
        _handler = new GetTruckQueryHandler(_mockDatabaseManager.Object, _mockMapper.Object);
    }


    [Fact]
    public async Task Handle_WithExistingTruck_ReturnsTruckDTO()
    {
        // Arrange
        var truckId = 1L;
        var truck = new Truck { Id = truckId };
        var truckDTO = new TruckDTO { Id = truckId };
        _mockDatabaseManager.Setup(x => x.ApplicationRepository.GetByIdAsyncWithIncludes(truckId, new string[] { nameof(Truck.Items) }, CancellationToken.None)).ReturnsAsync(truck);
        _mockMapper.Setup(x => x.MapEntityToDto(truck)).Returns(truckDTO);

        var query = new GetTruckQuery { TruckId = truckId };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(truckDTO.Id, result.Id);
    }

    [Fact]
    public async Task Handle_WithNonExistingTruck_ThrowsNotFoundException()
    {
        // Arrange
        long truckId = 1;
        var query = new GetTruckQuery { TruckId = truckId };

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        _mockDatabaseManager
            .Setup(x => x.ApplicationRepository.GetByIdAsyncWithIncludes(truckId, new string[] { nameof(Truck.Items) }, default))
            .ReturnsAsync((Truck)null);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(query, default));
    }
}
