using AuroraForecast.Aurora.Helpers;
using AuroraForecast.Aurora.Interfaces;
using AuroraForecast.Aurora.Services;
using AuroraForecast.Data;
using AuroraForecast.Locations.Interfaces;
using AuroraForecast.Locations.Repositories;
using AuroraForecast.Locations.Services;
using AuroraForecast.User.Interfaces;
using AuroraForecast.User.Repositories;
using AuroraForecast.User.Services;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
}

builder.Services.AddDbContext<AuroraDbContext>(options =>
{
    var sqlConnectionBuilder = new SqlConnectionStringBuilder(connectionString);
    
    if (!sqlConnectionBuilder.ConnectionString.Contains("Authentication"))
    {
        sqlConnectionBuilder.Authentication = SqlAuthenticationMethod.ActiveDirectoryDefault;
    }
    
    options.UseSqlServer(sqlConnectionBuilder.ConnectionString, sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null);
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(
                "http://localhost:3000",
                "https://localhost:3000",
                "https://calm-glacier-0ff898d10.3.azurestaticapps.net"
            )
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

// Add services to the container.
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ILocationService, LocationService>();
builder.Services.AddScoped<ILocationRepository, LocationRepository>();
builder.Services.AddScoped<IAuroraService, AuroraService>();
builder.Services.AddScoped<IPolicyProvider, PolicyProvider>();
builder.Services.AddScoped<AuroraApiWrapper>();
builder.Services.AddScoped<IAuroraApiWrapper, AuroraApiWrapperWithRetry>(provider =>
{
    var auroraApiWrapper = provider.GetRequiredService<AuroraApiWrapper>();
    var policyProvider = provider.GetRequiredService<IPolicyProvider>();
    return new AuroraApiWrapperWithRetry(auroraApiWrapper, policyProvider);
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Apply migrations automatically on startup (optional - good for development)
if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<AuroraDbContext>();
        db.Database.Migrate();
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowFrontend");

app.UseAuthorization();

app.MapControllers();

app.Run();
