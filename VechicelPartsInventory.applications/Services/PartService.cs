using VehiclePartsInventory.Application.DTOs.Parts;
using VehiclePartsInventory.Application.Interfaces;
using VehiclePartsInventory.Domain.Entities;

namespace VehiclePartsInventory.Application.Services;

public class PartService : IPartService
{
    private readonly IPartRepository _partRepository;

    public PartService(IPartRepository partRepository)
    {
        _partRepository = partRepository;
    }

    public async Task<List<PartResponseDto>> GetAllPartsAsync()
    {
        var parts = await _partRepository.GetAllPartsAsync();
        return parts.Select(MapToDto).ToList();
    }

    public async Task<PartResponseDto?> GetPartByIdAsync(int id)
    {
        var part = await _partRepository.GetPartByIdAsync(id);
        return part == null ? null : MapToDto(part);
    }

    public async Task<PartResponseDto> CreatePartAsync(PartCreateDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.PartNumber))
            throw new Exception("Part number is required.");

        if (string.IsNullOrWhiteSpace(dto.Name))
            throw new Exception("Part name is required.");

        if (await _partRepository.PartNumberExistsAsync(dto.PartNumber))
            throw new Exception("Part number already exists.");

        var part = new Part
        {
            PartNumber = dto.PartNumber.Trim().ToUpper(),
            Name = dto.Name,
            Description = dto.Description,
            Category = dto.Category,
            UnitPrice = dto.UnitPrice,
            StockQuantity = dto.StockQuantity,
            ReorderLevel = dto.ReorderLevel,
            CreatedAt = DateTime.UtcNow
        };

        await _partRepository.AddPartAsync(part);
        return MapToDto(part);
    }

    public async Task<PartResponseDto> UpdatePartAsync(int id, PartUpdateDto dto)
    {
        var part = await _partRepository.GetPartByIdAsync(id)
            ?? throw new Exception("Part not found.");

        part.Name = dto.Name;
        part.Description = dto.Description;
        part.Category = dto.Category;
        part.UnitPrice = dto.UnitPrice;
        part.StockQuantity = dto.StockQuantity;
        part.ReorderLevel = dto.ReorderLevel;

        await _partRepository.UpdatePartAsync(part);
        return MapToDto(part);
    }

    public async Task DeletePartAsync(int id)
    {
        var part = await _partRepository.GetPartByIdAsync(id)
            ?? throw new Exception("Part not found.");

        await _partRepository.DeletePartAsync(part);
    }

    private static PartResponseDto MapToDto(Part part) => new()
    {
        Id = part.Id,
        PartNumber = part.PartNumber,
        Name = part.Name,
        Description = part.Description,
        Category = part.Category,
        UnitPrice = part.UnitPrice,
        StockQuantity = part.StockQuantity,
        ReorderLevel = part.ReorderLevel,
        CreatedAt = part.CreatedAt
    };
}
