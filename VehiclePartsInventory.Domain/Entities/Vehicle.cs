namespace VehiclePartsInventory.Domain.Entities;

public class Vehicle
{
    public int Id { get; set; }

    public int CustomerId { get; set; }

    public Customer Customer { get; set; } = null!;

    public string VehicleNumber { get; set; } = string.Empty;

    public string VehicleType { get; set; } = string.Empty;

    public string Brand { get; set; } = string.Empty;

    public string Model { get; set; } = string.Empty;

    public int? Year { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}