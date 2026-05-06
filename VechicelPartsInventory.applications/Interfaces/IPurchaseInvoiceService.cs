using VehiclePartsInventory.Application.DTOs.PurchaseInvoices;

namespace VehiclePartsInventory.Application.Interfaces;

public interface IPurchaseInvoiceService
{
    Task<List<PurchaseInvoiceResponseDto>> GetAllAsync();
    Task<PurchaseInvoiceResponseDto?> GetByIdAsync(int id);
    Task<PurchaseInvoiceResponseDto> CreateAsync(CreatePurchaseInvoiceDto dto);
}
