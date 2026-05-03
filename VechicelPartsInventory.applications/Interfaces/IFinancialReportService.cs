using VehiclePartsInventory.Application.DTOs.Reports;

namespace VehiclePartsInventory.Application.Interfaces;

public interface IFinancialReportService
{
    Task<FinancialReportDto> GetDailyReportAsync(DateTime date);
    Task<FinancialReportDto> GetMonthlyReportAsync(int year, int month);
    Task<FinancialReportDto> GetYearlyReportAsync(int year);
}
