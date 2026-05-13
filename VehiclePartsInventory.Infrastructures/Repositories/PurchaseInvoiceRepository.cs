using Microsoft.EntityFrameworkCore;
using VehiclePartsInventory.Application.Interfaces;
using VehiclePartsInventory.Domain.Entities;
using VehiclePartsInventory.Infrastructure.Data;

namespace VehiclePartsInventory.Infrastructure.Repositories;

public class PurchaseInvoiceRepository : IPurchaseInvoiceRepository
{
    private readonly AppDbContext _context;

    public PurchaseInvoiceRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<PurchaseInvoice>> GetAllAsync()
    {
        return await _context.PurchaseInvoices
            .Include(pi => pi.Vendor)
            .Include(pi => pi.Items)
                .ThenInclude(i => i.Part)
            .OrderByDescending(pi => pi.PurchaseDate)
            .ToListAsync();
    }

    public async Task<PurchaseInvoice?> GetByIdAsync(int id)
    {
        return await _context.PurchaseInvoices
            .Include(pi => pi.Vendor)
            .Include(pi => pi.Items)
                .ThenInclude(i => i.Part)
            .FirstOrDefaultAsync(pi => pi.Id == id);
    }

    public async Task AddWithStockUpdateAsync(PurchaseInvoice invoice, List<Part> updatedParts)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            foreach (var part in updatedParts)
                _context.Parts.Update(part);

            _context.PurchaseInvoices.Add(invoice);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<string> GenerateInvoiceNumberAsync()
    {
        var count = await _context.PurchaseInvoices.CountAsync();
        return $"PI-{DateTime.UtcNow:yyyyMM}-{(count + 1):D4}";
    }
}
