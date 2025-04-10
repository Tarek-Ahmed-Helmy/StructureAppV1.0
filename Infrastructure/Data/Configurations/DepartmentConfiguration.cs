using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Models;

namespace Infrastructure.Data.Configurations;

internal sealed class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.ToTable(nameof(Department));

        builder.HasKey(d => d.Id);

        builder.Property(d => d.DepartmentName).HasMaxLength(60).IsRequired();
    }
}