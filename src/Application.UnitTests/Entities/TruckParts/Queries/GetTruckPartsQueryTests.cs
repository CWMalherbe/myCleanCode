using Application.Entities.TruckParts.Queries;
using Application.Exception;

namespace Application.UnitTests.Entities.TruckParts.Queries;
public class GetTruckPartsQueryHandlerTests
{
    private readonly Mock<IDatabaseManager<Truck>> _databaseManagerMock;
    private readonly Mock<IMapper<TruckPartDTO, TruckPart>> _mapperMock;
    private readonly GetTruckPartsQueryHandler _handler;

    public GetTruckPartsQueryHandlerTests()
    {
        _databaseManagerMock = new Mock<IDatabaseManager<Truck>>();
        _mapperMock = new Mock<IMapper<TruckPartDTO, TruckPart>>();
        _handler = new GetTruckPartsQueryHandler(_databaseManagerMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ExistingTruck_ReturnsListOfTruckPartDTOs()
    {
        // Arrange
        long truckId = 1234;
        var query = new GetTruckPartsQuery { TruckId = truckId };

        var truck = new Truck { Id = truckId, Items = new List<TruckPart> { new TruckPart { Id = 1, Name = "Engine" }, new TruckPart { Id = 2, Name = "Tire" } } };

        _databaseManagerMock.Setup(x => x.ApplicationRepository.GetByIdAsyncWithIncludes(query.TruckId, new string[] { nameof(Truck.Items) }, default)).ReturnsAsync(truck);

        _mapperMock.Setup(x => x.MapEntityToDto(It.IsAny<TruckPart>()))
            .Returns<TruckPart>(p => new TruckPartDTO { Id = p.Id, Name = p.Name });

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);

        Assert.Equal(1, result[0].Id);
        Assert.Equal("Engine", result[0].Name);

        Assert.Equal(2, result[1].Id);
        Assert.Equal("Tire", result[1].Name);
    }

    [Fact]
    public async Task Handle_NonExistingTruck_ThrowsNotFoundException()
    {
        // Arrange
        long truckId = 1234;
        var query = new GetTruckPartsQuery { TruckId = truckId };

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        _databaseManagerMock.Setup(x => x.ApplicationRepository.GetByIdAsyncWithIncludes(query.TruckId, new string[] { nameof(Truck.Items) }, default)).ReturnsAsync((Truck)null);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

        // Act + Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(query, CancellationToken.None));
    }
}