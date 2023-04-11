using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfigurations;

/// <summary>
/// Truck configuration
/// </summary>
public class TruckConfiguration : IEntityTypeConfiguration<Truck>
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="builder"></param>
    public void Configure(EntityTypeBuilder<Truck> builder)
    {
        builder.ToTable("Trucks");
        builder.Property(e => e.Name)
            .HasMaxLength(64)
            .IsRequired();
        builder.OwnsOne(e => e.Paint);
    }
}
