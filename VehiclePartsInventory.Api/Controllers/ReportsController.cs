using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehiclePartsInventory.Application.Interfaces;

namespace VehiclePartsInventory.Api.Controllers;

[ApiController]
[Route("api/reports")]
[Authorize]
public class ReportsController : ControllerBase
{
    private readonly ICustomerService _customerService;
    private readonly IFinancialReportService _financialReportService;

    public ReportsController(ICustomerService customerService, IFinancialReportService financialReportService)
    {
        _customerService = customerService;
        _financialReportService = financialReportService;
    }

    [HttpGet("customers")]
    [Authorize(Roles = "Staff,Admin")]
    public async Task<IActionResult> GetCustomerReport()
    {
        var result = await _customerService.GetCustomerReportAsync();
        return Ok(result);
    }

    [HttpGet("financial/daily")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetDailyReport([FromQuery] DateTime date)
    {
        var result = await _financialReportService.GetDailyReportAsync(date);
        return Ok(result);
    }

    [HttpGet("financial/monthly")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetMonthlyReport([FromQuery] int year, [FromQuery] int month)
    {
        var result = await _financialReportService.GetMonthlyReportAsync(year, month);
        return Ok(result);
    }

    [HttpGet("financial/yearly")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetYearlyReport([FromQuery] int year)
    {
        var result = await _financialReportService.GetYearlyReportAsync(year);
        return Ok(result);
    }
}