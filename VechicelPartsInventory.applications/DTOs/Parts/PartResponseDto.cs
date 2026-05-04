namespace VehiclePartsInventory.Application.DTOs.Parts;

public class PartResponseDto
{
    public int Id { get; set; }
    public string PartNumber { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public int StockQuantity { get; set; }
    public int ReorderLevel { get; set; }
    public bool IsLowStock => StockQuantity <= ReorderLevel;
    public DateTime CreatedAt { get; set; }
}
