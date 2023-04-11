namespace Application.UnitTests.Entities.TruckParts;
public class TruckPartMappingTests
{
    private readonly TruckPartMapping _mapper;

    public TruckPartMappingTests()
    {
        _mapper = new TruckPartMapping();
    }

    [Fact]
    public void MapDtoToEntity_Should_Return_Valid_Entity()
    {
        // Arrange
        var dtoEntity = new TruckPartDTO
        {
            Id = 1,
            Name = "Engine Oil Filter",
            ConditionValue = ConditionEnum.New,
            TruckId = 1,
            Code = "E-OF",
        };

        // Act
        var entity = _mapper.MapDtoToEntity(dtoEntity);

        // Assert
        Assert.Equal(dtoEntity.Id, entity.Id);
        Assert.Equal(dtoEntity.Name, entity.Name);
        Assert.Equal(dtoEntity.ConditionValue, entity.Condition);
        Assert.Equal(dtoEntity.TruckId, entity.TruckId);
        Assert.Equal(dtoEntity.Code, entity.Code);
    }

    [Fact]
    public void MapEntityToDto_Should_Return_Valid_Dto_Entity()
    {
        // Arrange
        var entity = new TruckPart
        {
            Id = 1,
            Name = "Engine Oil Filter",
            Condition = ConditionEnum.New,
            TruckId = 1,
            Code = "E-OF",
        };

        // Act
        var dtoEntity = _mapper.MapEntityToDto(entity);

        // Assert
        Assert.Equal(entity.Id, dtoEntity.Id);
        Assert.Equal(entity.Name, dtoEntity.Name);
        Assert.Equal(entity.Condition.ToString(), dtoEntity.Condition);
        Assert.Equal(entity.TruckId, dtoEntity.TruckId);
        Assert.Equal(entity.Code, dtoEntity.Code);
    }
}