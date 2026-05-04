namespace VehiclePartsInventory.Domain.Entities;

public class PurchaseInvoice
{
    public int Id { get; set; }
    public string InvoiceNumber { get; set; } = string.Empty;
    public int VendorId { get; set; }
    public Vendor Vendor { get; set; } = null!;
    public DateTime PurchaseDate { get; set; }
    public decimal TotalAmount { get; set; }
    public string Notes { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<PurchaseInvoiceItem> Items { get; set; } = new List<PurchaseInvoiceItem>();
}
