using VehiclePartsInventory.Api.Extensions;
using VehiclePartsInventory.Api.Middleware;
using VehiclePartsInventory.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add controllers
builder.Services.AddControllers();

// Add API explorer / OpenAPI
builder.Services.AddEndpointsApiExplorer();

// .NET 10 OpenAPI support
builder.Services.AddOpenApi();

// Add CORS for frontend connection
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .WithOrigins(
                "http://localhost:5173",
                "https://localhost:5173"
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// Add custom application services
builder.Services.AddApplicationServices(builder.Configuration);

// Add JWT authentication and authorization services
builder.Services.AddIdentityServices(builder.Configuration);

var app = builder.Build();

// Global exception handler
app.UseMiddleware<ExceptionMiddleware>();

// Seed database/admin user
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await DbSeeder.SeedAsync(context);
}

// OpenAPI endpoint for development
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Redirect HTTP to HTTPS
app.UseHttpsRedirection();

// Enable CORS before authentication/authorization
app.UseCors("AllowFrontend");

// Authentication and authorization
app.UseAuthentication();
app.UseAuthorization();

// Map controllers
app.MapControllers();

app.Run();