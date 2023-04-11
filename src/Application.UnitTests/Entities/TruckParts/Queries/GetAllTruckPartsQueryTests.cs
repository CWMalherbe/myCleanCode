using Application.Entities.TruckParts.Queries;
using Domain.Entities;

namespace Application.UnitTests.Entities.TruckParts.Queries;
public class GetAllTruckPartsQueryHandlerTests
{
    private readonly Mock<IDatabaseManager<TruckPart>> _databaseManagerMock = new();
    private readonly Mock<IMapper<TruckPartDTO, TruckPart>> _mapperMock = new();
    private readonly GetAllTruckPartsQueryHandler _handler;

    public GetAllTruckPartsQueryHandlerTests()
    {
        _handler = new GetAllTruckPartsQueryHandler(_databaseManagerMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ReturnsListOfTruckPartDTOs_WhenCalled()
    {
        // Arrange
        var truckParts = new List<TruckPart>
        {
            new TruckPart { Id = 1, Name = "Part 1", Code = "CODE-1", TruckId = 1 },
            new TruckPart { Id = 2, Name = "Part 2", Code = "CODE-2", TruckId = 1 },
            new TruckPart { Id = 3, Name = "Part 3", Code = "CODE-3", TruckId = 2 },
        };
        _databaseManagerMock.Setup(x => x.ApplicationRepository.GetAllEntitiesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(truckParts);
        _mapperMock.Setup(x => x.MapEntityToDto(It.IsAny<TruckPart>()))
            .Returns<TruckPart>(x => new TruckPartDTO { Id = x.Id, Name = x.Name, Code = x.Code, TruckId = x.TruckId });

        // Act
        var result = await _handler.Handle(new GetAllTruckPartsQuery(), CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<IList<TruckPartDTO>>(result);
        Assert.Equal(3, result.Count);
        Assert.Contains(result, x => x.Id == 1 && x.Name == "Part 1" && x.Code == "CODE-1" && x.TruckId == 1);
        Assert.Contains(result, x => x.Id == 2 && x.Name == "Part 2" && x.Code == "CODE-2" && x.TruckId == 1);
        Assert.Contains(result, x => x.Id == 3 && x.Name == "Part 3" && x.Code == "CODE-3" && x.TruckId == 2);
    }

    [Fact]
    public async Task Handle_ReturnsEmptyList_WhenNoTruckPartsFound()
    {
        // Arrange
        _databaseManagerMock.Setup(x => x.ApplicationRepository.GetAllEntitiesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<TruckPart>());

        // Act
        var result = await _handler.Handle(new GetAllTruckPartsQuery(), CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<IList<TruckPartDTO>>(result);
        Assert.Empty(result);
    }
}