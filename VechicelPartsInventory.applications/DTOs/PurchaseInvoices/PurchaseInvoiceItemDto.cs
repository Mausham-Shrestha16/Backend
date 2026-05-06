namespace VehiclePartsInventory.Application.DTOs.PurchaseInvoices;

public class PurchaseInvoiceItemDto
{
    public int PartId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}
