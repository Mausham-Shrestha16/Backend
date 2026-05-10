namespace VehiclePartsInventory.Application.DTOs.Customers;

public class VehicleCreateDto
{
    public string VehicleNumber { get; set; } = string.Empty;

    public string VehicleType { get; set; } = string.Empty;

    public string Brand { get; set; } = string.Empty;

    public string Model { get; set; } = string.Empty;

    public int? Year { get; set; }
}