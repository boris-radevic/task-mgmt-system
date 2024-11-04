using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TaskManagementSystem.Tasks.DataAccess
{
    public class TasksDbConfiguration : IEntityTypeConfiguration<Models.Task>
    {
        public void Configure(EntityTypeBuilder<Models.Task> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }
}