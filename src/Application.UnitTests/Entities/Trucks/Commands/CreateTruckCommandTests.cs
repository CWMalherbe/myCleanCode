using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Entities.TruckParts.Commands;
using Application.Entities.Trucks.Commands;

namespace Application.UnitTests.Entities.Trucks.Commands;
public class CreateTruckCommandTests
{
    private readonly Mock<IDatabaseManager<Truck>> _mockDatabaseManager;
    private readonly Mock<IMapper<TruckDTO, Truck>> _mockMapper;
    private readonly Mock<IValidator<TruckDTO, Truck>> _mockValidatorTruck;
    private readonly Mock<IValidator<TruckPartDTO, TruckPart>> _mockValidatorTruckPart;
    private readonly CreateTruckCommandHandler _handler;

    public CreateTruckCommandTests()
    {
        _mockDatabaseManager = new Mock<IDatabaseManager<Truck>>();
        _mockMapper = new Mock<IMapper<TruckDTO, Truck>>();
        _mockValidatorTruck = new Mock<IValidator<TruckDTO, Truck>>();
        _mockValidatorTruckPart = new Mock<IValidator<TruckPartDTO, TruckPart>>();
        _handler = new CreateTruckCommandHandler(_mockDatabaseManager.Object, _mockMapper.Object, _mockValidatorTruck.Object, _mockValidatorTruckPart.Object);
    }

    [Fact]
    public async Task Handle_ShouldInsertNewTruckIntoDatabase()
    {
        // Arrange
        var createTruckCommand = new CreateTruckCommand
        {
            Name = "Test Truck",
            Paint = Paint.Yellow.Hex,
            Items = new List<CreateTruckPartCommand>
            {
                new CreateTruckPartCommand
                {
                    Name = "Engine",
                    Code = "ENG123",
                    Condition = ConditionEnum.New,
                },
                new CreateTruckPartCommand
                {
                    Name = "Wheels",
                    Code = "WHL456",
                    Condition = ConditionEnum.Used,
                }
            }
        };

        var createdTruckId = 1L;
        _mockDatabaseManager.Setup(x => x.ApplicationRepository.InsertAsync(It.IsAny<Truck>(), CancellationToken.None))
            .Callback<Truck, CancellationToken>((entity, _) => entity.Id = createdTruckId);

        // Act
        var result = await _handler.Handle(createTruckCommand, CancellationToken.None);

        // Assert
        Assert.Equal(createdTruckId, result);
        _mockDatabaseManager.Verify(x => x.ApplicationRepository.InsertAsync(It.IsAny<Truck>(), CancellationToken.None), Times.Once);
        _mockDatabaseManager.Verify(x => x.ApplicationRepository.SaveChangesAsync(CancellationToken.None), Times.Once);
        _mockValidatorTruck.Verify(x => x.ValidateEntity(It.IsAny<Truck>()), Times.Once);
        _mockValidatorTruckPart.Verify(x => x.ValidateEntity(It.IsAny<TruckPart>()), Times.AtLeastOnce);
    }
}
