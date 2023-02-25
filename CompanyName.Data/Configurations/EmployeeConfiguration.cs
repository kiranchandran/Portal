using CompanyName.Data.Entity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace CompanyName.Data.Configurations
{
    internal class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.ToTable("Employee");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id).HasColumnName("Id").IsRequired(true).ValueGeneratedOnAdd();
            builder.Property(e => e.CreatedDate).HasColumnName("CreatedDate").IsRequired(true).HasDefaultValue(DateTime.UtcNow);

            builder.Property(e => e.DateOfBirth).HasColumnName("DateOfBirth").IsRequired(true);

            builder.Property(e => e.Name).HasColumnName("Name").IsRequired(true).HasMaxLength(255);

            builder.Property(e => e.Email).HasColumnName("Email").IsRequired(true).HasMaxLength(255);

            builder.Property(e => e.IsDeleted).HasColumnName("IsDeleted").IsRequired(true).HasDefaultValue(false);

            builder.HasQueryFilter(e => !e.IsDeleted);
        }
    }
}
