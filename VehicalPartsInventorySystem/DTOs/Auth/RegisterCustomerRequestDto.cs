using VehiclePartsInventory.Application.DTOs.Customers;

namespace VehiclePartsInventory.Application.DTOs.Auth;

public class RegisterCustomerRequestDto
{
    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public VehicleCreateDto Vehicle { get; set; } = new();
}