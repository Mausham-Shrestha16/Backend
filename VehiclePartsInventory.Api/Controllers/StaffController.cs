using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehiclePartsInventory.Application.DTOs.Staff;
using VehiclePartsInventory.Application.Interfaces;

namespace VehiclePartsInventory.Api.Controllers;

[ApiController]
[Route("api/staff")]
[Authorize(Roles = "Admin")]
public class StaffController : ControllerBase
{
    private readonly IStaffService _staffService;

    public StaffController(IStaffService staffService)
    {
        _staffService = staffService;
    }

    [HttpPost]
    public async Task<IActionResult> RegisterStaff(StaffRegisterDto dto)
    {
        var result = await _staffService.RegisterStaffAsync(dto);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllStaff()
    {
        var result = await _staffService.GetAllStaffAsync();
        return Ok(result);
    }

    [HttpPatch("{staffId:int}/status")]
    public async Task<IActionResult> UpdateStaffStatus(int staffId, UpdateStaffStatusDto dto)
    {
        var result = await _staffService.UpdateStaffStatusAsync(staffId, dto);
        return Ok(result);
    }
}
