using Microsoft.AspNetCore.Mvc;
using VehicalPartsInventorySystem.Application.DTOs.Auth;
using VehiclePartsInventory.Application.DTOs.Auth;
using VehiclePartsInventory.Application.Interfaces;

namespace VehiclePartsInventory.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register-customer")]
    public async Task<IActionResult> RegisterCustomer(RegisterCustomerRequestDto dto)
    {
        var result = await _authService.RegisterCustomerAsync(dto);
        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequestDto dto)
    {
        var result = await _authService.LoginAsync(dto);
        return Ok(result);
    }
}