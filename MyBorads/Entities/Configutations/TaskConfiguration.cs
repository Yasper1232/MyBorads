using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyBorads.Entities.Configutations
{
    public class TaskConfiguration : IEntityTypeConfiguration<Task>
    {
        public void Configure(EntityTypeBuilder<Task> ta)
        {
            ta.Property(wi => wi.Activity).HasMaxLength(200);
            ta.Property(wi => wi.RemaningWork).HasPrecision(14, 2);
        }
    }
}
