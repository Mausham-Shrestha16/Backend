using VehiclePartsInventory.Domain.Entities;

namespace VehiclePartsInventory.Application.Interfaces;

public interface IPartRepository
{
    Task<List<Part>> GetAllPartsAsync();
    Task<Part?> GetPartByIdAsync(int id);
    Task<bool> PartNumberExistsAsync(string partNumber);
    Task AddPartAsync(Part part);
    Task UpdatePartAsync(Part part);
    Task DeletePartAsync(Part part);
}
