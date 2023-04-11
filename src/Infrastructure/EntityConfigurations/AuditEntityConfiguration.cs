using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfigurations;
/// <summary>
/// Audit entity configuration
/// </summary>
public class AuditEntityConfiguration : IEntityTypeConfiguration<AuditEntity>
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="builder"></param>
    public void Configure(EntityTypeBuilder<AuditEntity> builder)
    {
        builder.ToTable("AuditTable");
    }
}
