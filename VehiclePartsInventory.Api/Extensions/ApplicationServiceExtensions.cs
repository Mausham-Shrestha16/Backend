using Microsoft.EntityFrameworkCore;
using VehicalPartsInventorySystem.Interfaces;
using VehiclePartsInventory.Application.Interfaces;
using VehiclePartsInventory.Application.Services;
using VehiclePartsInventory.Infrastructure.Data;
using VehiclePartsInventory.Infrastructure.Repositories;
using VehiclePartsInventory.Infrastructure.Services;

namespace VehiclePartsInventory.Api.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        IConfiguration config)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(config.GetConnectionString("DefaultConnection"));
        });

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IStaffRepository, StaffRepository>();
        services.AddScoped<IPartRepository, PartRepository>();

        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IStaffService, StaffService>();
        services.AddScoped<IPartService, PartService>();
        services.AddScoped<ITokenService, TokenService>();

        return services;
    }
}