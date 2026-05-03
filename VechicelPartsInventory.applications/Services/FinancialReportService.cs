using VehiclePartsInventory.Application.DTOs.Reports;
using VehiclePartsInventory.Application.Interfaces;

namespace VehiclePartsInventory.Application.Services;

public class FinancialReportService : IFinancialReportService
{
    private readonly ISalesInvoiceRepository _invoiceRepository;

    public FinancialReportService(ISalesInvoiceRepository invoiceRepository)
    {
        _invoiceRepository = invoiceRepository;
    }

    public async Task<FinancialReportDto> GetDailyReportAsync(DateTime date)
    {
        var from = DateTime.SpecifyKind(date.Date, DateTimeKind.Utc);
        var to = from.AddDays(1);

        var invoices = await _invoiceRepository.GetByDateRangeAsync(from, to);

        var breakdown = invoices
            .GroupBy(i => i.InvoiceDate.Hour)
            .OrderBy(g => g.Key)
            .Select(g => new FinancialPeriodDto
            {
                Period = $"{g.Key:D2}:00",
                Revenue = g.Sum(i => i.TotalAmount),
                Paid = g.Sum(i => i.PaidAmount),
                Credit = g.Sum(i => i.TotalAmount - i.PaidAmount),
                InvoiceCount = g.Count()
            }).ToList();

        return BuildReport(invoices.Sum(i => i.TotalAmount),
            invoices.Sum(i => i.PaidAmount), invoices.Count, breakdown);
    }

    public async Task<FinancialReportDto> GetMonthlyReportAsync(int year, int month)
    {
        var from = DateTime.SpecifyKind(new DateTime(year, month, 1), DateTimeKind.Utc);
        var to = from.AddMonths(1);

        var invoices = await _invoiceRepository.GetByDateRangeAsync(from, to);

        var breakdown = invoices
            .GroupBy(i => i.InvoiceDate.Day)
            .OrderBy(g => g.Key)
            .Select(g => new FinancialPeriodDto
            {
                Period = $"Day {g.Key}",
                Revenue = g.Sum(i => i.TotalAmount),
                Paid = g.Sum(i => i.PaidAmount),
                Credit = g.Sum(i => i.TotalAmount - i.PaidAmount),
                InvoiceCount = g.Count()
            }).ToList();

        return BuildReport(invoices.Sum(i => i.TotalAmount),
            invoices.Sum(i => i.PaidAmount), invoices.Count, breakdown);
    }

    public async Task<FinancialReportDto> GetYearlyReportAsync(int year)
    {
        var from = DateTime.SpecifyKind(new DateTime(year, 1, 1), DateTimeKind.Utc);
        var to = from.AddYears(1);

        var invoices = await _invoiceRepository.GetByDateRangeAsync(from, to);

        var breakdown = invoices
            .GroupBy(i => i.InvoiceDate.Month)
            .OrderBy(g => g.Key)
            .Select(g => new FinancialPeriodDto
            {
                Period = new DateTime(year, g.Key, 1).ToString("MMMM"),
                Revenue = g.Sum(i => i.TotalAmount),
                Paid = g.Sum(i => i.PaidAmount),
                Credit = g.Sum(i => i.TotalAmount - i.PaidAmount),
                InvoiceCount = g.Count()
            }).ToList();

        return BuildReport(invoices.Sum(i => i.TotalAmount),
            invoices.Sum(i => i.PaidAmount), invoices.Count, breakdown);
    }

    private static FinancialReportDto BuildReport(decimal totalRevenue, decimal totalPaid,
        int count, List<FinancialPeriodDto> breakdown) => new()
    {
        TotalRevenue = totalRevenue,
        TotalPaid = totalPaid,
        TotalCredit = totalRevenue - totalPaid,
        TotalInvoices = count,
        Breakdown = breakdown
    };
}
