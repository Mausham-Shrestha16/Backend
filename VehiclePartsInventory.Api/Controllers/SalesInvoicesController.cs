using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehiclePartsInventory.Application.DTOs.SalesInvoices;
using VehiclePartsInventory.Application.Interfaces;

namespace VehiclePartsInventory.Api.Controllers;

[ApiController]
[Route("api/sales-invoices")]
[Authorize(Roles = "Admin,Staff")]
public class SalesInvoicesController : ControllerBase
{
    private readonly ISalesInvoiceService _service;
    private readonly IEmailService _emailService;

    public SalesInvoicesController(ISalesInvoiceService service, IEmailService emailService)
    {
        _service = service;
        _emailService = emailService;
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
        if (result == null) return NotFound("Invoice not found.");
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateSalesInvoiceDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return Ok(result);
    }

    [HttpPatch("{id:int}/payment")]
    public async Task<IActionResult> UpdatePayment(int id, [FromBody] decimal paidAmount)
    {
        var result = await _service.UpdatePaidAmountAsync(id, paidAmount);
        return Ok(result);
    }

    [HttpPost("{id:int}/send-email")]
    public async Task<IActionResult> SendInvoiceEmail(int id)
    {
        await _emailService.SendInvoiceEmailAsync(id);
        return Ok(new { message = "Invoice email sent successfully." });
    }
}
