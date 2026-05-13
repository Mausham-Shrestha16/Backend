namespace VehiclePartsInventory.Application.DTOs.Customers;

public class CustomerDetailsDto
{
    public int CustomerId { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public List<VehicleInfoDto> Vehicles { get; set; } = new();

    public List<CustomerInvoiceHistoryDto> PurchaseHistory { get; set; } = new();
}

public class VehicleInfoDto
{
    public int VehicleId { get; set; }

    public string VehicleNumber { get; set; } = string.Empty;

    public string VehicleType { get; set; } = string.Empty;

    public string Brand { get; set; } = string.Empty;

    public string Model { get; set; } = string.Empty;

    public int? Year { get; set; }
}

public class CustomerInvoiceHistoryDto
{
    public int InvoiceId { get; set; }

    public string InvoiceNumber { get; set; } = string.Empty;

    public DateTime InvoiceDate { get; set; }

    public decimal TotalAmount { get; set; }

    public decimal PaidAmount { get; set; }

    public decimal CreditAmount { get; set; }

    public List<CustomerInvoiceItemHistoryDto> Items { get; set; } = new();
}

public class CustomerInvoiceItemHistoryDto
{
    public string PartName { get; set; } = string.Empty;

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal SubTotal { get; set; }
}
