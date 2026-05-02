using VehiclePartsInventory.Domain.Entities;

namespace VehiclePartsInventory.Application.Interfaces;

public interface IStaffRepository
{
    Task<List<AppUser>> GetAllStaffAsync();
    Task<AppUser?> GetStaffByIdAsync(int id);
    Task AddStaffAsync(AppUser user);
    Task UpdateStaffAsync(AppUser user);
}
