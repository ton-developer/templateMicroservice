using Application.Abstractions.Data;
using Domain.Abstractions;
using Infrastructure.Driven;
using Infrastructure.Driven.Interceptor;
using Infrastructure.Driven.Repositories;
using Microsoft.EntityFrameworkCore;
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