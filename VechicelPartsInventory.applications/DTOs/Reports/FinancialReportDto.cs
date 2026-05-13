namespace VehiclePartsInventory.Application.DTOs.Reports;

public class FinancialReportDto
{
    public decimal TotalRevenue { get; set; }
    public decimal TotalPaid { get; set; }
    public decimal TotalCredit { get; set; }
    public int TotalInvoices { get; set; }
    public List<FinancialPeriodDto> Breakdown { get; set; } = new();
}

public class FinancialPeriodDto
{
    public string Period { get; set; } = string.Empty;
    public decimal Revenue { get; set; }
    public decimal Paid { get; set; }
    public decimal Credit { get; set; }
    public int InvoiceCount { get; set; }
}
