using Microsoft.EntityFrameworkCore;
using VehiclePartsInventory.Application.Interfaces;
using VehiclePartsInventory.Domain.Entities;
using VehiclePartsInventory.Infrastructure.Data;

namespace VehiclePartsInventory.Infrastructure.Repositories;

public class PartRepository : IPartRepository
{
    private readonly AppDbContext _context;

    public PartRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Part>> GetAllPartsAsync()
    {
        return await _context.Parts.OrderBy(p => p.Name).ToListAsync();
    }

    public async Task<Part?> GetPartByIdAsync(int id)
    {
        return await _context.Parts.FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<bool> PartNumberExistsAsync(string partNumber)
    {
        var normalized = partNumber.Trim().ToUpper();
        return await _context.Parts.AnyAsync(p => p.PartNumber == normalized);
    }

    public async Task AddPartAsync(Part part)
    {
        _context.Parts.Add(part);
        await _context.SaveChangesAsync();
    }

    public async Task UpdatePartAsync(Part part)
    {
        _context.Parts.Update(part);
        await _context.SaveChangesAsync();
    }

    public async Task DeletePartAsync(Part part)
    {
        _context.Parts.Remove(part);
        await _context.SaveChangesAsync();
    }
}
