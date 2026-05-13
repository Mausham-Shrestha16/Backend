using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehiclePartsInventory.Application.Interfaces;

namespace VehiclePartsInventory.Api.Controllers;

[ApiController]
[Route("api/alerts")]
[Authorize(Roles = "Admin")]
public class AlertsController : ControllerBase
{
    private readonly IAlertService _alertService;

    public AlertsController(IAlertService alertService)
    {
        _alertService = alertService;
    }

    [HttpGet("low-stock")]
    public async Task<IActionResult> GetLowStock()
    {
        var result = await _alertService.GetLowStockPartsAsync();
        return Ok(result);
    }

    [HttpGet("overdue-credit")]
    public async Task<IActionResult> GetOverdueCredit()
    {
        var result = await _alertService.GetOverdueCreditCustomersAsync();
        return Ok(result);
    }

    [HttpPost("low-stock/send-email")]
    public async Task<IActionResult> SendLowStockEmail()
    {
        await _alertService.SendLowStockEmailAlertAsync();
        return Ok(new { message = "Low stock alert email sent." });
    }

    [HttpPost("overdue-credit/send-email")]
    public async Task<IActionResult> SendOverdueCreditEmail()
    {
        await _alertService.SendOverdueCreditEmailAlertAsync();
        return Ok(new { message = "Overdue credit reminder emails sent." });
    }
}
