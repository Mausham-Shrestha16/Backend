namespace VehiclePartsInventory.Domain.Entities;

public class SalesInvoice
{
    public int Id { get; set; }

    public string InvoiceNumber { get; set; } = string.Empty;

    public int CustomerId { get; set; }

    public Customer Customer { get; set; } = null!;

    public DateTime InvoiceDate { get; set; } = DateTime.UtcNow;

    public decimal TotalAmount { get; set; }

    public decimal PaidAmount { get; set; }

    public ICollection<SalesInvoiceItem> Items { get; set; } = new List<SalesInvoiceItem>();
}