using VehiclePartsInventory.Application.DTOs.Vendors;

namespace VehiclePartsInventory.Application.Interfaces;

public interface IVendorService
{
    Task<List<VendorResponseDto>> GetAllAsync();
    Task<VendorResponseDto?> GetByIdAsync(int id);
    Task<VendorResponseDto> CreateAsync(VendorCreateDto dto);
    Task<VendorResponseDto> UpdateAsync(int id, VendorUpdateDto dto);
    Task DeleteAsync(int id);
}
