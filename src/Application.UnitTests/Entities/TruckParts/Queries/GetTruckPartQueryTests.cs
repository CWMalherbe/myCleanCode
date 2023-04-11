using Application.Entities.TruckParts.Queries;
using Application.Exception;

namespace Application.UnitTests.Entities.TruckParts.Queries;
public class GetTruckPartQueryHandlerTests
{
    private readonly Mock<IDatabaseManager<TruckPart>> _databaseManagerMock;
    private readonly Mock<IMapper<TruckPartDTO, TruckPart>> _mapperMock;
    private readonly GetTruckPartQueryHandler _handler;

    public GetTruckPartQueryHandlerTests()
    {
        _databaseManagerMock = new Mock<IDatabaseManager<TruckPart>>();
        _mapperMock = new Mock<IMapper<TruckPartDTO, TruckPart>>();
        _handler = new GetTruckPartQueryHandler(_databaseManagerMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ValidRequest_ReturnsTruckPartDTO()
    {
        // Arrange
        var query = new GetTruckPartQuery { TruckPartId = 1, TruckId = 1 };
        var existingEntity = new TruckPart { Id = 1, TruckId = 1, Name = "Part 1", Code = "CODE1" };
        var expectedDto = new TruckPartDTO { Id = 1, Name = "Part 1", Code = "CODE1" };

        _databaseManagerMock.Setup(x => x.ApplicationRepository.GetByIdAsync(query.TruckPartId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingEntity);

        _mapperMock.Setup(x => x.MapEntityToDto(existingEntity)).Returns(expectedDto);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(expectedDto, result);
    }

    [Fact]
    public async Task Handle_NonExistingTruckPart_ThrowsNotFoundException()
    {
        // Arrange
        var query = new GetTruckPartQuery { TruckPartId = 1, TruckId = 1 };

        _databaseManagerMock.Setup(x => x.ApplicationRepository.GetByIdAsync(query.TruckPartId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((TruckPart?)null);

        // Act + Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(query, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_TruckPartNotBelongingToTruck_ThrowsNotFoundException()
    {
        // Arrange
        var query = new GetTruckPartQuery { TruckPartId = 1, TruckId = 2 };
        var existingEntity = new TruckPart { Id = 1, TruckId = 1, Name = "Part 1", Code = "CODE1" };

        _databaseManagerMock.Setup(x => x.ApplicationRepository.GetByIdAsync(query.TruckPartId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingEntity);

        // Act + Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(query, CancellationToken.None));
    }
}
