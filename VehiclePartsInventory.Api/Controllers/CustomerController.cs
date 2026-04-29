using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehiclePartsInventory.Application.DTOs.Customers;
using VehiclePartsInventory.Application.Interfaces;

namespace VehiclePartsInventory.Api.Controllers;

[ApiController]
[Route("api/customers")]
[Authorize]
public class CustomersController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomersController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpPost]
    [Authorize(Roles = "Staff,Admin")]
    public async Task<IActionResult> RegisterCustomerByStaff(CustomerCreateDto dto)
    {
        var result = await _customerService.RegisterCustomerByStaffAsync(dto);
        return Ok(result);
    }

    [HttpGet("{customerId:int}")]
    [Authorize(Roles = "Staff,Admin")]
    public async Task<IActionResult> GetCustomerDetails(int customerId)
    {
        var result = await _customerService.GetCustomerDetailsAsync(customerId);

        if (result == null)
            return NotFound("Customer not found.");

        return Ok(result);
    }

    [HttpGet("search")]
    [Authorize(Roles = "Staff,Admin")]
    public async Task<IActionResult> SearchCustomers([FromQuery] string keyword)
    {
        var result = await _customerService.SearchCustomersAsync(keyword);
        return Ok(result);
    }
}