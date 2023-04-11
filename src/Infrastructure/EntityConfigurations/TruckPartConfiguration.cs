using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfigurations;

/// <summary>
/// Truck part configuration
/// </summary>
public class TruckPartConfiguration : IEntityTypeConfiguration<TruckPart>
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="builder"></param>
    public void Configure(EntityTypeBuilder<TruckPart> builder)
    {
        builder.ToTable("TruckParts");
        builder.Property(e => e.Name)
            .HasMaxLength(64)
            .IsRequired();
    }
}
