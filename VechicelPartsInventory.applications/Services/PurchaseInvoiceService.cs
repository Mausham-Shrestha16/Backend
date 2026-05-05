using VehiclePartsInventory.Application.DTOs.PurchaseInvoices;
using VehiclePartsInventory.Application.Interfaces;
using VehiclePartsInventory.Domain.Entities;

namespace VehiclePartsInventory.Application.Services;

public class PurchaseInvoiceService : IPurchaseInvoiceService
{
    private readonly IPurchaseInvoiceRepository _invoiceRepository;
    private readonly IPartRepository _partRepository;

    public PurchaseInvoiceService(IPurchaseInvoiceRepository invoiceRepository, IPartRepository partRepository)
    {
        _invoiceRepository = invoiceRepository;
        _partRepository = partRepository;
    }

    public async Task<List<PurchaseInvoiceResponseDto>> GetAllAsync()
    {
        var invoices = await _invoiceRepository.GetAllAsync();
        return invoices.Select(MapToDto).ToList();
    }

    public async Task<PurchaseInvoiceResponseDto?> GetByIdAsync(int id)
    {
        var invoice = await _invoiceRepository.GetByIdAsync(id);
        return invoice == null ? null : MapToDto(invoice);
    }

    public async Task<PurchaseInvoiceResponseDto> CreateAsync(CreatePurchaseInvoiceDto dto)
    {
        if (dto.Items == null || dto.Items.Count == 0)
            throw new Exception("At least one item is required.");

        var items = new List<PurchaseInvoiceItem>();
        var partsToUpdate = new List<Part>();
        decimal total = 0;

        foreach (var itemDto in dto.Items)
        {
            var part = await _partRepository.GetPartByIdAsync(itemDto.PartId)
                ?? throw new Exception($"Part with ID {itemDto.PartId} not found.");

            part.StockQuantity += itemDto.Quantity;
            partsToUpdate.Add(part);

            total += itemDto.Quantity * itemDto.UnitPrice;

            items.Add(new PurchaseInvoiceItem
            {
                PartId = itemDto.PartId,
                Quantity = itemDto.Quantity,
                UnitPrice = itemDto.UnitPrice
            });
        }

        var invoiceNumber = await _invoiceRepository.GenerateInvoiceNumberAsync();

        var invoice = new PurchaseInvoice
        {
            InvoiceNumber = invoiceNumber,
            VendorId = dto.VendorId,
            PurchaseDate = DateTime.SpecifyKind(dto.PurchaseDate, DateTimeKind.Utc),
            TotalAmount = total,
            Notes = dto.Notes,
            Items = items,
            CreatedAt = DateTime.UtcNow
        };

        await _invoiceRepository.AddWithStockUpdateAsync(invoice, partsToUpdate);
        var saved = await _invoiceRepository.GetByIdAsync(invoice.Id)
            ?? throw new Exception("Failed to retrieve saved invoice.");

        return MapToDto(saved);
    }

    private static PurchaseInvoiceResponseDto MapToDto(PurchaseInvoice inv) => new()
    {
        Id = inv.Id,
        InvoiceNumber = inv.InvoiceNumber,
        VendorId = inv.VendorId,
        VendorName = inv.Vendor?.Name ?? "",
        PurchaseDate = inv.PurchaseDate,
        TotalAmount = inv.TotalAmount,
        Notes = inv.Notes,
        CreatedAt = inv.CreatedAt,
        Items = inv.Items.Select(i => new PurchaseInvoiceItemResponseDto
        {
            Id = i.Id,
            PartId = i.PartId,
            PartName = i.Part?.Name ?? "",
            PartNumber = i.Part?.PartNumber ?? "",
            Quantity = i.Quantity,
            UnitPrice = i.UnitPrice
        }).ToList()
    };
}
