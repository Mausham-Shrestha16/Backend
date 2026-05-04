using VehiclePartsInventory.Application.DTOs.Parts;

namespace VehiclePartsInventory.Application.Interfaces;

public interface IPartService
{
    Task<List<PartResponseDto>> GetAllPartsAsync();
    Task<PartResponseDto?> GetPartByIdAsync(int id);
    Task<PartResponseDto> CreatePartAsync(PartCreateDto dto);
    Task<PartResponseDto> UpdatePartAsync(int id, PartUpdateDto dto);
    Task DeletePartAsync(int id);
}
