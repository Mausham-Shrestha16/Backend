using VehiclePartsInventory.Application.DTOs.SalesInvoices;
using VehiclePartsInventory.Application.Interfaces;
using VehiclePartsInventory.Domain.Entities;

namespace VehiclePartsInventory.Application.Services;

public class SalesInvoiceService : ISalesInvoiceService
{
    private readonly ISalesInvoiceRepository _invoiceRepository;

    public SalesInvoiceService(ISalesInvoiceRepository invoiceRepository)
    {
        _invoiceRepository = invoiceRepository;
    }

    public async Task<List<SalesInvoiceResponseDto>> GetAllAsync()
    {
        var invoices = await _invoiceRepository.GetAllAsync();
        return invoices.Select(MapToDto).ToList();
    }

    public async Task<SalesInvoiceResponseDto?> GetByIdAsync(int id)
    {
        var invoice = await _invoiceRepository.GetByIdAsync(id);
        return invoice == null ? null : MapToDto(invoice);
    }

    public async Task<SalesInvoiceResponseDto> CreateAsync(CreateSalesInvoiceDto dto)
    {
        if (dto.Items == null || dto.Items.Count == 0)
            throw new Exception("At least one item is required.");

        var items = dto.Items.Select(i => new SalesInvoiceItem
        {
            PartName = i.PartName,
            Quantity = i.Quantity,
            UnitPrice = i.UnitPrice
        }).ToList();

        var total = items.Sum(i => i.Quantity * i.UnitPrice);

        if (dto.PaidAmount > total)
            throw new Exception("Paid amount cannot exceed total amount.");

        var invoiceNumber = await _invoiceRepository.GenerateInvoiceNumberAsync();

        var invoice = new SalesInvoice
        {
            InvoiceNumber = invoiceNumber,
            CustomerId = dto.CustomerId,
            InvoiceDate = DateTime.SpecifyKind(dto.InvoiceDate, DateTimeKind.Utc),
            TotalAmount = total,
            PaidAmount = dto.PaidAmount,
            Items = items
        };

        await _invoiceRepository.AddAsync(invoice);

        var saved = await _invoiceRepository.GetByIdAsync(invoice.Id)
            ?? throw new Exception("Failed to retrieve saved invoice.");

        return MapToDto(saved);
    }

    public async Task<SalesInvoiceResponseDto> UpdatePaidAmountAsync(int id, decimal paidAmount)
    {
        var invoice = await _invoiceRepository.GetByIdAsync(id)
            ?? throw new Exception("Invoice not found.");

        if (paidAmount > invoice.TotalAmount)
            throw new Exception("Paid amount cannot exceed total amount.");

        invoice.PaidAmount = paidAmount;
        await _invoiceRepository.UpdateAsync(invoice);

        return MapToDto(invoice);
    }

    private static SalesInvoiceResponseDto MapToDto(SalesInvoice inv) => new()
    {
        Id = inv.Id,
        InvoiceNumber = inv.InvoiceNumber,
        CustomerId = inv.CustomerId,
        CustomerName = inv.Customer?.AppUser?.FullName ?? "",
        CustomerEmail = inv.Customer?.AppUser?.Email ?? "",
        InvoiceDate = inv.InvoiceDate,
        TotalAmount = inv.TotalAmount,
        PaidAmount = inv.PaidAmount,
        Items = inv.Items.Select(i => new SalesInvoiceItemResponseDto
        {
            Id = i.Id,
            PartName = i.PartName,
            Quantity = i.Quantity,
            UnitPrice = i.UnitPrice
        }).ToList()
    };
}
