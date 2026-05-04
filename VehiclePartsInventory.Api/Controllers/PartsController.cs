using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehiclePartsInventory.Application.DTOs.Parts;
using VehiclePartsInventory.Application.Interfaces;

namespace VehiclePartsInventory.Api.Controllers;

[ApiController]
[Route("api/parts")]
[Authorize(Roles = "Admin,Staff")]
public class PartsController : ControllerBase
{
    private readonly IPartService _partService;

    public PartsController(IPartService partService)
    {
        _partService = partService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllParts()
    {
        var result = await _partService.GetAllPartsAsync();
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetPart(int id)
    {
        var result = await _partService.GetPartByIdAsync(id);
        if (result == null)
            return NotFound("Part not found.");
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreatePart(PartCreateDto dto)
    {
        var result = await _partService.CreatePartAsync(dto);
        return Ok(result);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdatePart(int id, PartUpdateDto dto)
    {
        var result = await _partService.UpdatePartAsync(id, dto);
        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeletePart(int id)
    {
        await _partService.DeletePartAsync(id);
        return Ok(new { message = "Part deleted successfully." });
    }
}
