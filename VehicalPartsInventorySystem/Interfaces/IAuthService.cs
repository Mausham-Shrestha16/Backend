using VehicalPartsInventorySystem.Application.DTOs.Auth;
using VehiclePartsInventory.Application.DTOs.Auth;

namespace VehiclePartsInventory.Application.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterCustomerAsync(RegisterCustomerRequestDto dto);

    Task<AuthResponseDto> LoginAsync(LoginRequestDto dto);
}