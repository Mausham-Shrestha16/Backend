using Microsoft.EntityFrameworkCore;
using VehiclePartsInventory.Application.Interfaces;
using VehiclePartsInventory.Domain.Entities;
using VehiclePartsInventory.Infrastructure.Data;

namespace VehiclePartsInventory.Infrastructure.Repositories;

public class VendorRepository : IVendorRepository
{
    private readonly AppDbContext _context;

    public VendorRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Vendor>> GetAllAsync()
    {
        return await _context.Vendors.OrderBy(v => v.Name).ToListAsync();
    }

    public async Task<Vendor?> GetByIdAsync(int id)
    {
        return await _context.Vendors.FirstOrDefaultAsync(v => v.Id == id);
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        var normalized = email.Trim().ToLower();
        if (string.IsNullOrEmpty(normalized)) return false;
        return await _context.Vendors.AnyAsync(v => v.Email == normalized);
    }

    public async Task AddAsync(Vendor vendor)
    {
        _context.Vendors.Add(vendor);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Vendor vendor)
    {
        _context.Vendors.Update(vendor);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Vendor vendor)
    {
        _context.Vendors.Remove(vendor);
        await _context.SaveChangesAsync();
    }
}
