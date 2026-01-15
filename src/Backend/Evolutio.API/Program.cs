using Evolutio.API.Converters;
using Evolutio.API.Filters;
using Evolutio.API.Middlewares;
using Evolutio.API.Services.Cookie;
using Evolutio.API.Token;
using Evolutio.Application;
using Evolutio.Communication;
using Evolutio.Domain.Security.Tokens;
using Evolutio.Domain.Services.Cookie;
using Evolutio.Infrastructure;
using Evolutio.Infrastructure.Extensions;
using Evolutio.Infrastructure.Migrations;
using Microsoft.OpenApi.Models;

var AUTHENTICATION_TYPE = "Bearer";

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Services.AddCors(options => options.AddPolicy(Configuration.CorsPolicyName, policy =>
policy.WithOrigins([Configuration.BackendUrl, Configuration.FrontendUrl]).AllowAnyMethod().AllowAnyHeader().AllowCredentials()));

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
// Add token provider
builder.Services.AddScoped<ITokenProvider, HttpContextTokenValue>();
// Add context accessor
builder.Services.AddHttpContextAccessor();
// Add cookie service
builder.Services.AddScoped<ICookieService, CookieService>();
// Add low case routing
builder.Services.AddRouting(options => options.LowercaseUrls = true);
// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => 
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. 
Enter 'Bearer' [space] and then your token in the text input below.
Example: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = AUTHENTICATION_TYPE
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = AUTHENTICATION_TYPE
                },
                Scheme = "oauth2",
                Name = AUTHENTICATION_TYPE,
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<CultureMiddleware>();

app.UseCors(Configuration.CorsPolicyName);
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
