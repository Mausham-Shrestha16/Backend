using Microsoft.EntityFrameworkCore;
using VehiclePartsInventory.Domain.Entities;
using VehiclePartsInventory.Domain.Enums;

namespace VehiclePartsInventory.Infrastructure.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        await context.Database.MigrateAsync();

        if (!await context.Users.AnyAsync(u => u.Role == UserRole.Admin))
        {
            var admin = new AppUser
            {
                FullName = "System Admin",
                Email = "admin@vehicleparts.com",
                PhoneNumber = "9800000000",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                Role = UserRole.Admin,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            context.Users.Add(admin);
            await context.SaveChangesAsync();
        }
    }
}