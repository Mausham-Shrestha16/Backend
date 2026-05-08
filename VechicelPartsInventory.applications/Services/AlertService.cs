using VehiclePartsInventory.Application.DTOs.Alerts;
using VehiclePartsInventory.Application.Interfaces;

namespace VehiclePartsInventory.Application.Services;

public class AlertService : IAlertService
{
    private readonly IPartRepository _partRepository;
    private readonly ISalesInvoiceRepository _invoiceRepository;
    private readonly IEmailService _emailService;

    public AlertService(
        IPartRepository partRepository,
        ISalesInvoiceRepository invoiceRepository,
        IEmailService emailService)
    {
        _partRepository = partRepository;
        _invoiceRepository = invoiceRepository;
        _emailService = emailService;
    }

    public async Task<LowStockAlertDto> GetLowStockPartsAsync()
    {
        var parts = await _partRepository.GetAllPartsAsync();
        var lowStock = parts
            .Where(p => p.StockQuantity <= p.ReorderLevel)
            .Select(p => new LowStockPartDto
            {
                Id = p.Id,
                PartNumber = p.PartNumber,
                Name = p.Name,
                Category = p.Category,
                StockQuantity = p.StockQuantity,
                ReorderLevel = p.ReorderLevel
            }).ToList();

        return new LowStockAlertDto { Count = lowStock.Count, Parts = lowStock };
    }

    public async Task<OverdueCreditAlertDto> GetOverdueCreditCustomersAsync()
    {
        var invoices = await _invoiceRepository.GetAllAsync();

        var customers = invoices
            .Where(i => i.TotalAmount > i.PaidAmount)
            .GroupBy(i => i.Customer)
            .Where(g => g.Key?.AppUser != null)
            .Select(g => new OverdueCreditCustomerDto
            {
                CustomerId = g.Key!.Id,
                FullName = g.Key.AppUser!.FullName,
                Email = g.Key.AppUser.Email,
                PhoneNumber = g.Key.AppUser.PhoneNumber,
                TotalCredit = g.Sum(i => i.TotalAmount - i.PaidAmount),
                OverdueInvoiceCount = g.Count()
            }).ToList();

        return new OverdueCreditAlertDto { Count = customers.Count, Customers = customers };
    }

    public async Task SendLowStockEmailAlertAsync()
    {
        await _emailService.SendLowStockAlertAsync();
    }

    public async Task SendOverdueCreditEmailAlertAsync()
    {
        await _emailService.SendOverdueCreditRemindersAsync();
    }
}
