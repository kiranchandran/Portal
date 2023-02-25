using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using CompanyName.Data.Entity;

namespace CompanyName.Data.Configurations
{
    internal class DepartmentConfiguration : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.ToTable("Department");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id).HasColumnName("Id").IsRequired(true).ValueGeneratedOnAdd();
            builder.Property(e => e.Name).HasColumnName("Name").IsRequired(true).HasMaxLength(255);
            builder.Property(e => e.IsDeleted).HasColumnName("IsDeleted").IsRequired(true).HasDefaultValue(false);

            builder.HasQueryFilter(e => !e.IsDeleted);
        }
    }
}
