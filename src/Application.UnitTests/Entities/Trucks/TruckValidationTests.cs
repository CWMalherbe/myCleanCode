using Application.Exception;

namespace Application.UnitTests.Entities.Trucks;
public class TruckValidationTests
{
    [Fact]
    public void ValidateDtoEntity_EmptyName_ThrowsValidationFailedException()
    {
        // Arrange
        var validator = new TruckValidation();
        var dto = new TruckDTO { Name = "" };

        // Act + Assert
        Assert.Throws<ValidationFailedException>(() => validator.ValidateDtoEntity(dto));
    }

    [Fact]
    public void ValidateDtoEntity_LongName_ThrowsValidationFailedException()
    {
        // Arrange
        var validator = new TruckValidation();
        var dto = new TruckDTO { Name = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Fusce fringilla nisl vel orci ultricies, eu blandit elit commodo. Sed tristique sapien vel lorem vulputate fermentum." };

        // Act + Assert
        Assert.Throws<ValidationFailedException>(() => validator.ValidateDtoEntity(dto));
    }

    [Fact]
    public void ValidateDtoEntity_ValidDto_DoesNotThrowException()
    {
        // Arrange
        var validator = new TruckValidation();
        var dto = new TruckDTO { Name = "Valid Name" };

        // Act + Assert
        validator.ValidateDtoEntity(dto);
    }

    [Fact]
    public void ValidateEntity_EmptyName_ThrowsValidationFailedException()
    {
        // Arrange
        var validator = new TruckValidation();
        var entity = new Truck { Name = "" };

        // Act + Assert
        Assert.Throws<ValidationFailedException>(() => validator.ValidateEntity(entity));
    }

    [Fact]
    public void ValidateEntity_LongName_ThrowsValidationFailedException()
    {
        // Arrange
        var validator = new TruckValidation();
        var entity = new Truck { Name = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Fusce fringilla nisl vel orci ultricies, eu blandit elit commodo. Sed tristique sapien vel lorem vulputate fermentum." };

        // Act + Assert
        Assert.Throws<ValidationFailedException>(() => validator.ValidateEntity(entity));
    }

    [Fact]
    public void ValidateEntity_ValidEntity_DoesNotThrowException()
    {
        // Arrange
        var validator = new TruckValidation();
        var entity = new Truck { Name = "Valid Name" };

        // Act + Assert
        validator.ValidateEntity(entity);
    }
}