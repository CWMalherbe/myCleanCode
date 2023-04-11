using Application.Entities.TruckParts.Commands;

namespace Application.UnitTests.Entities.TruckParts.Commands;
public class CreateTruckPartCommandHandlerTests
{

    [Fact]
    public async Task Handle_ValidRequest_CreatesNewTruckPart()
    {
        // Arrange
        var truckPart = new CreateTruckPartCommand
        {
            TruckId = 1,
            Name = "Test Part",
            Code = "TP01"
        };
        long createdEntityId = 123;
        var validator = new Mock<IValidator<TruckPartDTO, TruckPart>>();
        validator.Setup(v => v.ValidateEntity(It.IsAny<TruckPart>())).Verifiable();

        var databaseManager = new Mock<IDatabaseManager<TruckPart>>();
        databaseManager.Setup(dm => dm.ApplicationRepository.InsertAsync(It.IsAny<TruckPart>(), CancellationToken.None))
            .Callback<TruckPart, CancellationToken>((entity, token) =>
            {
                entity.Id = createdEntityId;
            })
            .Returns(Task.CompletedTask);

        var handler = new CreateTruckPartCommandHandler(databaseManager.Object, validator.Object);

        // Act
        var result = await handler.Handle(truckPart, CancellationToken.None);

        // Assert
        validator.Verify();
        databaseManager.Verify(dm => dm.ApplicationRepository.InsertAsync(It.IsAny<TruckPart>(), CancellationToken.None), Times.Once);
        databaseManager.Verify(dm => dm.ApplicationRepository.SaveChangesAsync(CancellationToken.None), Times.Once);
        Assert.Equal(createdEntityId, result);
    }
}