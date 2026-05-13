namespace VehiclePartsInventory.Application.DTOs.Customers;

public class UpdateCustomerProfileDto
{
    public string FullName { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;
}