using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehiclePartsInventory.Application.DTOs.PurchaseInvoices;
using VehiclePartsInventory.Application.Interfaces;

namespace VehiclePartsInventory.Api.Controllers;

[ApiController]
[Route("api/purchase-invoices")]
[Authorize(Roles = "Admin,Staff")]
public class PurchaseInvoicesController : ControllerBase
{
    private readonly IPurchaseInvoiceService _service;

    public PurchaseInvoicesController(IPurchaseInvoiceService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        if (result == null)
            return NotFound("Invoice not found.");
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreatePurchaseInvoiceDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return Ok(result);
    }
}
