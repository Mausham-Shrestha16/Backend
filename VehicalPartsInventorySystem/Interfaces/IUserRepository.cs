using VehiclePartsInventory.Domain.Entities;

namespace VehiclePartsInventory.Application.Interfaces;

public interface IUserRepository
{
    Task<bool> EmailExistsAsync(string email);

    Task<bool> PhoneExistsAsync(string phoneNumber);

    Task<AppUser?> GetByEmailAsync(string email);
}