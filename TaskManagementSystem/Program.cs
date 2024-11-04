using FluentValidation;
using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Middlewares;
using TaskManagementSystem.Shared;
using TaskManagementSystem.Tasks;
using TaskManagementSystem.Tasks.DataAccess;
using TaskManagementSystem.Tasks.DataAccess.Contracts;
using TaskManagementSystem.Tasks.DataAccess.Rabbitmq;
using TaskManagementSystem.Tasks.DataAccess.Repositories;
using TaskManagementSystem.Tasks.Resources;
using TaskManagementSystem.Tasks.Validation;

var builder = WebApplication.CreateBuilder(args);

var connstring = builder.Configuration.GetConnectionString("Connection");
var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
//var appname = builder.Configuration.GetSection("AppName").Value;
//var envtype = builder.Configuration.GetSection("EnvironmentType").Value;
//var configOptions = builder.Configuration.GetSection(ConfigurationOptions.ConfigKey);
// Add services to the container.

builder.Services.Configure<RabbitMqConfiguration>(builder.Configuration.GetSection(RabbitMqConfiguration.keyName));

builder.Services.AddDbContextFactory<TasksDbContext>(options =>
{
    options.UseSqlServer(connstring);
});

builder.Services.AddSingleton(typeof(RabbitMqService));
builder.Services.AddSingleton<IEventPublisher, TestProducer>();

//Consumers
builder.Services.AddSingleton<IEventConsumer, AddTaskConsumer>();
builder.Services.AddSingleton<IEventConsumer, UpdateTaskConsumer>();
builder.Services.AddSingleton<IEventConsumer, DLQRetryConsumer>();

builder.Services.AddHostedService<ConsumerHostedService>();

//Producers
builder.Services.AddHostedService<AddTaskProducer>();
builder.Services.AddHostedService<UpdateTaskProducer>();

builder.Services.AddScoped<IValidator<AddTaskDto>, AddTaskDtoValidator>();
builder.Services.AddScoped<IValidator<TaskUpdateDto>, TaskUpdateDtoValidator>();

builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<ITaskService, TaskService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

try
{
    var app = builder.Build();

    // Configure the HTTP request pipeline.

    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseHttpExceptionMiddleware();
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<TasksDbContext>();
        dbContext.Database.Migrate();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    throw;
}
finally
{
}