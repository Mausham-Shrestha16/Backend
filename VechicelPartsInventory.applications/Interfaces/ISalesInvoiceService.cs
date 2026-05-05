using VehiclePartsInventory.Application.DTOs.SalesInvoices;

namespace VehiclePartsInventory.Application.Interfaces;

public interface ISalesInvoiceService
{
    Task<List<SalesInvoiceResponseDto>> GetAllAsync();
    Task<SalesInvoiceResponseDto?> GetByIdAsync(int id);
    Task<SalesInvoiceResponseDto> CreateAsync(CreateSalesInvoiceDto dto);
    Task<SalesInvoiceResponseDto> UpdatePaidAmountAsync(int id, decimal paidAmount);
}
