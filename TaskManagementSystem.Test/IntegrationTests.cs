using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using TaskManagementSystem.Tasks.DataAccess;

namespace TaskManagementSystem.Test
{
    public class IntegrationTests : IClassFixture<TasksApplication<Program, TasksDbContext>>
    {
        private readonly TasksApplication<Program, TasksDbContext> _contactsApplication;
        private TasksDbContext _context;
        private HttpClient _client;

        public IntegrationTests(TasksApplication<Program, TasksDbContext> contactsApplication)
        {
            _contactsApplication = contactsApplication;
            _client = contactsApplication.CreateClient();

            var options = new DbContextOptionsBuilder<TasksDbContext>()
                .UseSqlServer(_contactsApplication._sqlServerContainer.GetConnectionString());

            _context = new TasksDbContext();
        }

        [Fact]
        public async Task Testing_IsAlwaysValid_Is200()
        {
        }
    }
}