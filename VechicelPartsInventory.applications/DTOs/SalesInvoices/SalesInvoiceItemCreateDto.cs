namespace VehiclePartsInventory.Application.DTOs.SalesInvoices;

public class SalesInvoiceItemCreateDto
{
    public string PartName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}
