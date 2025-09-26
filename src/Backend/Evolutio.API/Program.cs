using Evolutio.API.Converters;
using Evolutio.API.Filters;
using Evolutio.API.Middlewares;
using Evolutio.Application;
using Evolutio.Infrastructure;
using Evolutio.Infrastructure.Extensions;
using Evolutio.Infrastructure.Migrations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(new StringConverter()));

// Add exception filter to handle exceptions globally
builder.Services.AddMvc(options =>
    options.Filters.Add(typeof(ExceptionFilter)));
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Add infrastructure services dependency injection
builder.Services.AddInfrastructure(builder.Configuration);
// Add application services dependency injection
builder.Services.AddApplication(builder.Configuration);
// Add low case routing
builder.Services.AddRouting(options => options.LowercaseUrls = true);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseMiddleware<CultureMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

MigrateDatabase();

app.Run();

void MigrateDatabase() 
{
    if (builder.Configuration.IsUnitTestEnvironment())
        return;

    var connectionString = builder.Configuration.ConnectionString();

    var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();

    DatabaseMigration.Migrate(connectionString, serviceScope.ServiceProvider);
}

public partial class Program
{
    protected Program() { }
}
