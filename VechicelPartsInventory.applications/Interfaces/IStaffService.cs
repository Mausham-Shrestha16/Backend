using VehiclePartsInventory.Application.DTOs.Staff;

namespace VehiclePartsInventory.Application.Interfaces;

public interface IStaffService
{
    Task<StaffResponseDto> RegisterStaffAsync(StaffRegisterDto dto);
    Task<List<StaffResponseDto>> GetAllStaffAsync();
    Task<StaffResponseDto> UpdateStaffStatusAsync(int staffId, UpdateStaffStatusDto dto);
}
