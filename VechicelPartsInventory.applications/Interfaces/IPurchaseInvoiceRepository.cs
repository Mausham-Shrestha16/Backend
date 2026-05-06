using VehiclePartsInventory.Domain.Entities;

namespace VehiclePartsInventory.Application.Interfaces;

public interface IPurchaseInvoiceRepository
{
    Task<List<PurchaseInvoice>> GetAllAsync();
    Task<PurchaseInvoice?> GetByIdAsync(int id);
    Task AddAsync(PurchaseInvoice invoice);
    Task<string> GenerateInvoiceNumberAsync();
}
