namespace VehiclePartsInventory.Application.DTOs.Alerts;

public class LowStockAlertDto
{
    public int Count { get; set; }
    public List<LowStockPartDto> Parts { get; set; } = new();
}

public class LowStockPartDto
{
    public int Id { get; set; }
    public string PartNumber { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public int StockQuantity { get; set; }
    public int ReorderLevel { get; set; }
}
