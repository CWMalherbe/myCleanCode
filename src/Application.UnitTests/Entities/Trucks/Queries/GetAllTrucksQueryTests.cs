using Application.Entities.Trucks.Queries;

namespace Application.UnitTests.Entities.Trucks.Queries;
public class GetAllTrucksQueryTests
{
    private readonly Mock<IDatabaseManager<Truck>> _mockDatabaseManager;
    private readonly Mock<IMapper<TruckDTO, Truck>> _mockMapper;
    private readonly GetAllTrucksQueryHandler _handler;

    public GetAllTrucksQueryTests()
    {
        _mockDatabaseManager = new Mock<IDatabaseManager<Truck>>();
        _mockMapper = new Mock<IMapper<TruckDTO, Truck>>();
        _handler = new GetAllTrucksQueryHandler(_mockMapper.Object, _mockDatabaseManager.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnListOfTruckDTOs_WhenCalled()
    {
        // Arrange
        var truck1 = new Truck { Id = 1, Name = "Truck 1" };
        var truck2 = new Truck { Id = 2, Name = "Truck 2" };
        var trucks = new List<Truck> { truck1, truck2 };
        _mockDatabaseManager.Setup(m => m.ApplicationRepository.GetAllEntitiesAsync(default)).ReturnsAsync(trucks);
        var truckDTO1 = new TruckDTO { Id = 1, Name = "Truck 1 DTO" };
        var truckDTO2 = new TruckDTO { Id = 2, Name = "Truck 2 DTO" };
        _mockMapper.Setup(m => m.MapEntityToDto(truck1)).Returns(truckDTO1);
        _mockMapper.Setup(m => m.MapEntityToDto(truck2)).Returns(truckDTO2);

        // Act
        var result = await _handler.Handle(new GetAllTrucksQuery(), default);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<List<TruckDTO>>(result);
        Assert.Equal(2, result.Count);
        Assert.Equal(truckDTO1, result[0]);
        Assert.Equal(truckDTO2, result[1]);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoTrucksExist()
    {
        // Arrange
        var trucks = new List<Truck>();
        _mockDatabaseManager.Setup(m => m.ApplicationRepository.GetAllEntitiesAsync(default)).ReturnsAsync(trucks);

        // Act
        var result = await _handler.Handle(new GetAllTrucksQuery(), default);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<List<TruckDTO>>(result);
        Assert.Empty(result);
    }
}
