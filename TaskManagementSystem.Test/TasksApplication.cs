using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MsSql;
using Testcontainers.RabbitMq;

namespace TaskManagementSystem.Test
{
    public class TasksApplication<TProgram, TContext> : WebApplicationFactory<TProgram>, IAsyncLifetime where TProgram : class where TContext : DbContext
    {
        private readonly string _environment;
        public readonly MsSqlContainer _sqlServerContainer;
        public readonly RabbitMqContainer _rabbitMqContainer;

        public TasksApplication()
        {
            /// TODO: We'll get back to this later
            //var initScriptPath = Path.Combine(Directory.GetCurrentDirectory(), "init-integration.sql");

            _environment = "Local";
            _sqlServerContainer = new MsSqlBuilder()
                .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
                .WithPortBinding("1443")
                .WithExposedPort("21433")
                .WithPassword("test")
                .WithCleanUp(true)
                .WithAutoRemove(true)
                .Build();

            _rabbitMqContainer = new RabbitMqBuilder()
                .WithHostname("localhost")
                .WithUsername("user")
                .WithPassword("password")
                .Build();
        }

        public Task InitializeAsync()
        {
            var task = _sqlServerContainer.StartAsync();
            return task;
        }

        Task IAsyncLifetime.DisposeAsync()
        {
            return _sqlServerContainer.DisposeAsync().AsTask();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            try
            {
                builder.UseEnvironment(_environment);
                builder.ConfigureServices(services =>
                {
                    services.RemoveDbContext<TContext>();
                    var connectionString = _sqlServerContainer.GetConnectionString();

                    services.AddDbContext<TContext>(options =>
                    {
                        options.UseSqlServer(connectionString);
                    });

                    // We need to add here sql dependency injection
                });
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}