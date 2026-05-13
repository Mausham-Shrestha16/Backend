namespace VehiclePartsInventory.Application.DTOs.Alerts;

public class OverdueCreditAlertDto
{
    public int Count { get; set; }
    public List<OverdueCreditCustomerDto> Customers { get; set; } = new();
}

public class OverdueCreditCustomerDto
{
    public int CustomerId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public decimal TotalCredit { get; set; }
    public int OverdueInvoiceCount { get; set; }
}
