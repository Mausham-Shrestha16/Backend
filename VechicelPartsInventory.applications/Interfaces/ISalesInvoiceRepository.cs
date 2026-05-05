using VehiclePartsInventory.Domain.Entities;

namespace VehiclePartsInventory.Application.Interfaces;

public interface ISalesInvoiceRepository
{
    Task<List<SalesInvoice>> GetAllAsync();
    Task<SalesInvoice?> GetByIdAsync(int id);
    Task AddAsync(SalesInvoice invoice);
    Task UpdateAsync(SalesInvoice invoice);
    Task<string> GenerateInvoiceNumberAsync();
    Task<List<SalesInvoice>> GetByDateRangeAsync(DateTime from, DateTime to);
}
