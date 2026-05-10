using Microsoft.EntityFrameworkCore;
using VehiclePartsInventory.Application.Interfaces;
using VehiclePartsInventory.Domain.Entities;
using VehiclePartsInventory.Infrastructure.Data;

namespace VehiclePartsInventory.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        var normalizedEmail = email.Trim().ToLower();

        return await _context.Users.AnyAsync(u => u.Email == normalizedEmail);
    }

    public async Task<bool> PhoneExistsAsync(string phoneNumber)
    {
        return await _context.Users.AnyAsync(u => u.PhoneNumber == phoneNumber);
    }

    public async Task<AppUser?> GetByEmailAsync(string email)
    {
        var normalizedEmail = email.Trim().ToLower();

        return await _context.Users.FirstOrDefaultAsync(u => u.Email == normalizedEmail);
    }
}