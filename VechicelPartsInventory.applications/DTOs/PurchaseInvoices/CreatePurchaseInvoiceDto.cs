namespace VehiclePartsInventory.Application.DTOs.PurchaseInvoices;

public class CreatePurchaseInvoiceDto
{
    public int VendorId { get; set; }
    public DateTime PurchaseDate { get; set; }
    public string Notes { get; set; } = string.Empty;
    public List<PurchaseInvoiceItemDto> Items { get; set; } = new();
}
