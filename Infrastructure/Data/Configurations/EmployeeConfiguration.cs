using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Models;

namespace Infrastructure.Data.Configurations;

internal sealed class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable(nameof(Employee));

        builder.HasKey(e => e.Id);
        builder.Property(e => e.FirstName).HasMaxLength(20).IsRequired();
        builder.Property(e => e.LastName).HasMaxLength(20).IsRequired();
        builder.Property(e => e.Title).IsRequired();
        builder.Property(e => e.Salary).HasColumnType("decimal(18,2)");

        builder.HasOne(e => e.Department)
               .WithMany(d => d.Employees)
               .HasForeignKey(e => e.DepartmentId)
               .OnDelete(DeleteBehavior.SetNull);
    }
}
