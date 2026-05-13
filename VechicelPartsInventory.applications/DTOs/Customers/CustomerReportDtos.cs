namespace VehiclePartsInventory.Application.DTOs.Reports;

public class CustomerReportDto
{
    public int TotalCustomers { get; set; }

    public int RegularCustomers { get; set; }

    public int HighSpenders { get; set; }

    public int PendingCreditCustomers { get; set; }

    public List<CustomerReportItemDto> RegularCustomerList { get; set; } = new();

    public List<CustomerReportItemDto> HighSpenderList { get; set; } = new();

    public List<CustomerReportItemDto> PendingCreditCustomerList { get; set; } = new();
}

public class CustomerReportItemDto
{
    public int CustomerId { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public int InvoiceCount { get; set; }

    public decimal TotalSpent { get; set; }

    public decimal PendingCreditAmount { get; set; }
}