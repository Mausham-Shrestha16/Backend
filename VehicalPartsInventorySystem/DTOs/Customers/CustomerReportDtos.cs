namespace VehiclePartsInventory.Application.DTOs.Reports;

public class CustomerReportDto
{
    public int TotalCustomers { get; set; }

    public int RegularCustomers { get; set; }

    public int HighSpenders { get; set; }

    public int PendingCreditCustomers { get; set; }
}