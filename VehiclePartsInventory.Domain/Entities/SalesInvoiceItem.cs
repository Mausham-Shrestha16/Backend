namespace VehiclePartsInventory.Domain.Entities;

public class SalesInvoiceItem
{
    public int Id { get; set; }

    public int SalesInvoiceId { get; set; }

    public SalesInvoice SalesInvoice { get; set; } = null!;

    public string PartName { get; set; } = string.Empty;

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }
}