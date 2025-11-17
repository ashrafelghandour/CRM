using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infrastructure.Data.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.Email)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.Phone)
            .HasMaxLength(20);

        builder.Property(c => c.Address)
            .HasMaxLength(200);

        builder.Property(c => c.CompanyName)
            .HasMaxLength(100);

        builder.Property(c => c.CreatedBy)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.CreatedAt)
            .IsRequired();

        builder.Property(c => c.UpdateAt);

        builder.Property(c => c.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        // Indexes
        builder.HasIndex(c => c.Email)
            .IsUnique();

        // Relationships
        builder.HasMany(c => c.Feedbacks)
            .WithOne(cf => cf.Customer)
            .HasForeignKey(cf => cf.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(c => c.Tickets)
            .WithOne(t => t.Customer)
            .HasForeignKey(t => t.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}