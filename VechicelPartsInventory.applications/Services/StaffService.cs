using VehiclePartsInventory.Application.DTOs.Staff;
using VehiclePartsInventory.Application.Interfaces;
using VehiclePartsInventory.Domain.Entities;
using VehiclePartsInventory.Domain.Enums;

namespace VehiclePartsInventory.Application.Services;

public class StaffService : IStaffService
{
    private readonly IStaffRepository _staffRepository;
    private readonly IUserRepository _userRepository;

    public StaffService(IStaffRepository staffRepository, IUserRepository userRepository)
    {
        _staffRepository = staffRepository;
        _userRepository = userRepository;
    }

    public async Task<StaffResponseDto> RegisterStaffAsync(StaffRegisterDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.FullName))
            throw new Exception("Full name is required.");

        if (string.IsNullOrWhiteSpace(dto.Email))
            throw new Exception("Email is required.");

        if (await _userRepository.EmailExistsAsync(dto.Email))
            throw new Exception("Email already exists.");

        if (await _userRepository.PhoneExistsAsync(dto.PhoneNumber))
            throw new Exception("Phone number already exists.");

        var user = new AppUser
        {
            FullName = dto.FullName,
            Email = dto.Email.Trim().ToLower(),
            PhoneNumber = dto.PhoneNumber,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = UserRole.Staff,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        await _staffRepository.AddStaffAsync(user);

        return MapToDto(user);
    }

    public async Task<List<StaffResponseDto>> GetAllStaffAsync()
    {
        var staffList = await _staffRepository.GetAllStaffAsync();
        return staffList.Select(MapToDto).ToList();
    }

    public async Task<StaffResponseDto> UpdateStaffStatusAsync(int staffId, UpdateStaffStatusDto dto)
    {
        var user = await _staffRepository.GetStaffByIdAsync(staffId)
            ?? throw new Exception("Staff member not found.");

        user.IsActive = dto.IsActive;
        await _staffRepository.UpdateStaffAsync(user);

        return MapToDto(user);
    }

    private static StaffResponseDto MapToDto(AppUser user) => new()
    {
        Id = user.Id,
        FullName = user.FullName,
        Email = user.Email,
        PhoneNumber = user.PhoneNumber,
        Role = user.Role.ToString(),
        IsActive = user.IsActive,
        CreatedAt = user.CreatedAt
    };
}
