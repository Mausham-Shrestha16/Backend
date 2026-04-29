namespace VehiclePartsInventory.Application.DTOs.Customers;

public class CustomerSearchResultDto
{
    public int CustomerId { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public string VehicleNumber { get; set; } = string.Empty;
}