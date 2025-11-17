using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infrastructure.Data.Configurations;

public class SystemUserConfiguration : IEntityTypeConfiguration<SystemUser>
{
    public void Configure(EntityTypeBuilder<SystemUser> builder)
    {
        builder.Property(su => su.EmployeeId)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(su => su.Department)
            .HasMaxLength(50);

        builder.Property(su => su.HireDate)
            .IsRequired();

        builder.HasIndex(su => su.EmployeeId)
            .IsUnique();
    }
}