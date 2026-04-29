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

    public ReportsController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpGet("customers")]
    [Authorize(Roles = "Staff,Admin")]
    public async Task<IActionResult> GetCustomerReport()
    {
        var result = await _customerService.GetCustomerReportAsync();
        return Ok(result);
    }
}