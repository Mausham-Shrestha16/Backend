using VehiclePartsInventory.Application.DTOs.Vendors;
using VehiclePartsInventory.Application.Interfaces;
using VehiclePartsInventory.Domain.Entities;

namespace VehiclePartsInventory.Application.Services;

public class VendorService : IVendorService
{
    private readonly IVendorRepository _vendorRepository;

    public VendorService(IVendorRepository vendorRepository)
    {
        _vendorRepository = vendorRepository;
    }

    public async Task<List<VendorResponseDto>> GetAllAsync()
    {
        var vendors = await _vendorRepository.GetAllAsync();
        return vendors.Select(MapToDto).ToList();
    }

    public async Task<VendorResponseDto?> GetByIdAsync(int id)
    {
        var vendor = await _vendorRepository.GetByIdAsync(id);
        return vendor == null ? null : MapToDto(vendor);
    }

    public async Task<VendorResponseDto> CreateAsync(VendorCreateDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
            throw new Exception("Vendor name is required.");

        if (!string.IsNullOrWhiteSpace(dto.Email) && await _vendorRepository.EmailExistsAsync(dto.Email))
            throw new Exception("A vendor with this email already exists.");

        var vendor = new Vendor
        {
            Name = dto.Name,
            ContactPerson = dto.ContactPerson,
            Phone = dto.Phone,
            Email = dto.Email.Trim().ToLower(),
            Address = dto.Address,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        await _vendorRepository.AddAsync(vendor);
        return MapToDto(vendor);
    }

    public async Task<VendorResponseDto> UpdateAsync(int id, VendorUpdateDto dto)
    {
        var vendor = await _vendorRepository.GetByIdAsync(id)
            ?? throw new Exception("Vendor not found.");

        vendor.Name = dto.Name;
        vendor.ContactPerson = dto.ContactPerson;
        vendor.Phone = dto.Phone;
        vendor.Email = dto.Email.Trim().ToLower();
        vendor.Address = dto.Address;
        vendor.IsActive = dto.IsActive;

        await _vendorRepository.UpdateAsync(vendor);
        return MapToDto(vendor);
    }

    public async Task DeleteAsync(int id)
    {
        var vendor = await _vendorRepository.GetByIdAsync(id)
            ?? throw new Exception("Vendor not found.");

        await _vendorRepository.DeleteAsync(vendor);
    }

    private static VendorResponseDto MapToDto(Vendor v) => new()
    {
        Id = v.Id,
        Name = v.Name,
        ContactPerson = v.ContactPerson,
        Phone = v.Phone,
        Email = v.Email,
        Address = v.Address,
        IsActive = v.IsActive,
        CreatedAt = v.CreatedAt
    };
}
