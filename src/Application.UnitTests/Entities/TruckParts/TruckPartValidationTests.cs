using Application.Exception;

namespace Application.UnitTests.Entities.TruckParts;
public class TruckPartValidationTests
{
    private readonly TruckPartValidation _validator = new TruckPartValidation();

    [Fact]
    public void ValidateDtoEntity_EmptyName_ThrowsValidationFailedException()
    {
        // Arrange
        var dtoEntity = new TruckPartDTO { Name = "" };

        // Act & Assert
        Assert.Throws<ValidationFailedException>(() => _validator.ValidateDtoEntity(dtoEntity));
    }

    [Fact]
    public void ValidateDtoEntity_LongName_ThrowsValidationFailedException()
    {
        // Arrange
        var dtoEntity = new TruckPartDTO { Name = new string('a', 65) };

        // Act & Assert
        Assert.Throws<ValidationFailedException>(() => _validator.ValidateDtoEntity(dtoEntity));
    }

    [Fact]
    public void ValidateDtoEntity_ValidDtoEntity_DoesNotThrowException()
    {
        // Arrange
        var dtoEntity = new TruckPartDTO { Name = "Valid Truck Part" };

        // Act & Assert
        _validator.ValidateDtoEntity(dtoEntity);
    }

    [Fact]
    public void ValidateEntity_EmptyName_ThrowsValidationFailedException()
    {
        // Arrange
        var entity = new TruckPart { Name = "" };

        // Act & Assert
        Assert.Throws<ValidationFailedException>(() => _validator.ValidateEntity(entity));
    }

    [Fact]
    public void ValidateEntity_LongName_ThrowsValidationFailedException()
    {
        // Arrange
        var entity = new TruckPart { Name = new string('a', 65) };

        // Act & Assert
        Assert.Throws<ValidationFailedException>(() => _validator.ValidateEntity(entity));
    }

    [Fact]
    public void ValidateEntity_ValidEntity_DoesNotThrowException()
    {
        // Arrange
        var entity = new TruckPart { Name = "Valid Truck Part" };

        // Act & Assert
        _validator.ValidateEntity(entity);
    }
}