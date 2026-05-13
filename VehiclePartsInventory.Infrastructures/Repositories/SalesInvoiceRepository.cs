using Microsoft.EntityFrameworkCore;
using VehiclePartsInventory.Application.Interfaces;
using VehiclePartsInventory.Domain.Entities;
using VehiclePartsInventory.Infrastructure.Data;

namespace VehiclePartsInventory.Infrastructure.Repositories;

public class SalesInvoiceRepository : ISalesInvoiceRepository
{
    private readonly AppDbContext _context;

    public SalesInvoiceRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<SalesInvoice>> GetAllAsync()
    {
        return await _context.SalesInvoices
            .Include(s => s.Customer).ThenInclude(c => c.AppUser)
            .Include(s => s.Items)
            .OrderByDescending(s => s.InvoiceDate)
            .ToListAsync();
    }

    public async Task<SalesInvoice?> GetByIdAsync(int id)
    {
        return await _context.SalesInvoices
            .Include(s => s.Customer).ThenInclude(c => c.AppUser)
            .Include(s => s.Items)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task AddAsync(SalesInvoice invoice)
    {
        _context.SalesInvoices.Add(invoice);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(SalesInvoice invoice)
    {
        _context.SalesInvoices.Update(invoice);
        await _context.SaveChangesAsync();
    }

    public async Task<string> GenerateInvoiceNumberAsync()
    {
        var count = await _context.SalesInvoices.CountAsync();
        return $"SI-{DateTime.UtcNow:yyyyMM}-{(count + 1):D4}";
    }

    public async Task<List<SalesInvoice>> GetByDateRangeAsync(DateTime from, DateTime to)
    {
        return await _context.SalesInvoices
            .Include(s => s.Customer).ThenInclude(c => c.AppUser)
            .Include(s => s.Items)
            .Where(s => s.InvoiceDate >= from && s.InvoiceDate < to)
            .OrderBy(s => s.InvoiceDate)
            .ToListAsync();
    }
}
