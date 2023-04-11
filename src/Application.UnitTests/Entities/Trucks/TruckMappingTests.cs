namespace Application.UnitTests.Entities.Trucks;
public class TruckMappingTests
{
    private readonly IMapper<TruckPartDTO, TruckPart> _mockTruckPartMapper;
    private readonly TruckMapping _truckMapping;

    public TruckMappingTests()
    {
        // Arrange
        _mockTruckPartMapper = Mock.Of<IMapper<TruckPartDTO, TruckPart>>();
        _truckMapping = new TruckMapping(_mockTruckPartMapper);
    }

    [Fact]
    public void MapDtoToEntity_ReturnsValidTruck()
    {
        // Arrange
        var dto = new TruckDTO
        {
            Id = 1,
            Name = "Test Truck",
            Paint = Paint.Yellow.Hex,
            Items = new List<TruckPartDTO>
            {
                new TruckPartDTO { Id = 1, Name = "Engine_Valve", ConditionValue = ConditionEnum.New, TruckId = 1, Code = "ENG01" },
                new TruckPartDTO { Id = 2, Name = "Engine_Piston", ConditionValue = ConditionEnum.Used, TruckId = 1, Code = "TIR01" }
            }
        };

        // Setup mock mapper behavior
        Mock.Get(_mockTruckPartMapper).Setup(m => m.MapDtoToEntity(It.IsAny<TruckPartDTO>()))
                                      .Returns<TruckPartDTO>(dto => new TruckPart { Id = dto.Id, Name = dto.Name });

        // Act
        var result = _truckMapping.MapDtoToEntity(dto);

        // Assert
        Assert.NotNull(result.Items);
        Assert.Equal(dto.Id, result.Id);
        Assert.Equal(dto.Name, result.Name);
        Assert.Equal(Paint.Yellow, result.Paint);
        Assert.Equal(dto.Items.Count, result.Items.Count);
        Assert.All(result.Items, i => Assert.StartsWith("Engine", i.Name));
    }

    [Fact]
    public void MapEntityToDto_ReturnsValidTruckDTO()
    {
        // Arrange
        var entity = new Truck
        {
            Id = 1,
            Name = "Test Truck",
            Paint = Paint.Yellow,
            Items = new List<TruckPart>
            {
                new TruckPart { Id = 1, Name = "Engine_Valve", Condition = ConditionEnum.New, TruckId = 1, Code = "ENG01" },
                new TruckPart { Id = 2, Name = "Engine_Piston", Condition = ConditionEnum.Used, TruckId = 1, Code = "TIR01" }
            }
        };

        // Setup mock mapper behavior
        Mock.Get(_mockTruckPartMapper).Setup(m => m.MapEntityToDto(It.IsAny<TruckPart>()))
                                      .Returns<TruckPart>(e => new TruckPartDTO { Id = e.Id, Name = e.Name });

        // Act
        var result = _truckMapping.MapEntityToDto(entity);

        // Assert
        Assert.NotNull(result.Items);
        Assert.Equal(entity.Id, result.Id);
        Assert.Equal(entity.Name, result.Name);
        Assert.Equal(Paint.Yellow.Hex, result.Paint);
        Assert.Equal(entity.Items.Count, result.Items.Count);
        Assert.All(result.Items, i => Assert.StartsWith("Engine", i.Name));
    }
}