using Application.Abstractions.Data;
using Domain.Abstractions;
using Infrastructure.Driven;
using Infrastructure.Driven.Interceptor;
using Infrastructure.Driven.Jobs;
using Infrastructure.Driven.Repositories;
using Medallion.Threading.Postgres;
using Microsoft.EntityFrameworkCore;
using Quartz;
using Web.Behaviors;

var applicationAssembly = typeof(Application.AssemblyReference).Assembly;
var infrastructureAssembly = typeof(Infrastructure.Driven.AssemblyReference).Assembly;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(applicationAssembly);
    config.AddOpenBehavior(typeof(TransactionalCommandBehavior<,>));
});

builder.Services.AddSingleton<AggregateRootEventInterceptor>();
builder.Services.AddDbContext<ApplicationDbContext>(
        (serviceProvider, options) =>
{
    var interceptor = serviceProvider.GetRequiredService<AggregateRootEventInterceptor>();
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
        .AddInterceptors(interceptor);
});

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<OutboxRepository>(sp =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    return new OutboxRepository(connectionString!);
});

builder.Services.AddScoped<PostgresDistributedLock>(sp =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    return new PostgresDistributedLock(new PostgresAdvisoryLockKey("JobLock", allowHashing: true), connectionString!);
});

builder.Services.AddQuartz(options =>
{
    var jobKey = JobKey.Create(nameof(OutboxMessageProcessorJob));
    options.AddJob<OutboxMessageProcessorJob>(jobKey)
        .AddTrigger(trigger =>
            trigger
                .ForJob(jobKey)
                .WithSimpleSchedule(s => 
                    s
                        .WithInterval(TimeSpan.FromMilliseconds(250))
                        .RepeatForever())
            );
});

builder.Services.AddQuartzHostedService(options =>
{
    options.WaitForJobsToComplete = true;
});

var app = builder.Build();

// Migrate the database on startup
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});


app.Run();