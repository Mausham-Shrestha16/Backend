using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
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

    [HttpGet("me")]
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> GetMyProfile()
    {
        var appUserId = GetLoggedInUserId();

        var result = await _customerService.GetMyProfileAsync(appUserId);

        if (result == null)
            return NotFound("Customer profile not found.");

        return Ok(result);
    }

    [HttpPut("me")]
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> UpdateMyProfile(UpdateCustomerProfileDto dto)
    {
        var appUserId = GetLoggedInUserId();

        var result = await _customerService.UpdateMyProfileAsync(appUserId, dto);

        if (result == null)
            return NotFound("Customer profile not found.");

        return Ok(result);
    }

    [HttpPost("me/vehicles")]
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> AddMyVehicle(VehicleCreateDto dto)
    {
        var appUserId = GetLoggedInUserId();

        var result = await _customerService.AddMyVehicleAsync(appUserId, dto);

        if (result == null)
            return NotFound("Customer profile not found.");

        return Ok(result);
    }

    [HttpPut("me/vehicles/{vehicleId:int}")]
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> UpdateMyVehicle(int vehicleId, VehicleCreateDto dto)
    {
        var appUserId = GetLoggedInUserId();

        var result = await _customerService.UpdateMyVehicleAsync(appUserId, vehicleId, dto);

        if (result == null)
            return NotFound("Vehicle not found for this customer.");

        return Ok(result);
    }

    [HttpDelete("me/vehicles/{vehicleId:int}")]
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> DeleteMyVehicle(int vehicleId)
    {
        var appUserId = GetLoggedInUserId();

        var deleted = await _customerService.DeleteMyVehicleAsync(appUserId, vehicleId);

        if (!deleted)
            return NotFound("Vehicle not found for this customer.");

        return Ok(new { message = "Vehicle deleted successfully." });
    }

    [HttpGet("me/history")]
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> GetMyPurchaseHistory()
    {
        var appUserId = GetLoggedInUserId();

        var result = await _customerService.GetMyPurchaseHistoryAsync(appUserId);

        return Ok(result);
    }

    private int GetLoggedInUserId()
    {
        var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(userIdValue))
            throw new Exception("User ID was not found in token.");

        return int.Parse(userIdValue);
    }
}