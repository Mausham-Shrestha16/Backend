namespace VehiclePartsInventory.Application.DTOs.SalesInvoices;

public class CreateSalesInvoiceDto
{
    public int CustomerId { get; set; }
    public DateTime InvoiceDate { get; set; }
    public decimal PaidAmount { get; set; }
    public List<SalesInvoiceItemCreateDto> Items { get; set; } = new();
}
