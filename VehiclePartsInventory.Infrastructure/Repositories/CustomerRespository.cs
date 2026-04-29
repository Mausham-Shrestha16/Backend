using Microsoft.EntityFrameworkCore;
using VehiclePartsInventory.Application.DTOs.Customers;
using VehiclePartsInventory.Application.DTOs.Reports;
using VehiclePartsInventory.Application.Interfaces;
using VehiclePartsInventory.Domain.Entities;
using VehiclePartsInventory.Infrastructure.Data;

namespace VehiclePartsInventory.Infrastructure.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly AppDbContext _context;

    public CustomerRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> VehicleNumberExistsAsync(string vehicleNumber)
    {
        var normalizedVehicleNumber = vehicleNumber.Trim().ToLower();

        return await _context.Vehicles
            .AnyAsync(v => v.VehicleNumber.ToLower() == normalizedVehicleNumber);
    }

    public async Task AddCustomerAsync(Customer customer)
    {
        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();
    }

    public async Task<CustomerDetailsDto?> GetCustomerDetailsAsync(int customerId)
    {
        return await _context.Customers
            .Include(c => c.AppUser)
            .Include(c => c.Vehicles)
            .Include(c => c.SalesInvoices)
            .Where(c => c.Id == customerId)
            .Select(c => new CustomerDetailsDto
            {
                CustomerId = c.Id,
                FullName = c.AppUser.FullName,
                Email = c.AppUser.Email,
                PhoneNumber = c.AppUser.PhoneNumber,
                Address = c.Address,

                Vehicles = c.Vehicles.Select(v => new VehicleInfoDto
                {
                    VehicleId = v.Id,
                    VehicleNumber = v.VehicleNumber,
                    VehicleType = v.VehicleType,
                    Brand = v.Brand,
                    Model = v.Model,
                    Year = v.Year
                }).ToList(),

                PurchaseHistory = c.SalesInvoices.Select(s => new CustomerInvoiceHistoryDto
                {
                    InvoiceId = s.Id,
                    InvoiceNumber = s.InvoiceNumber,
                    InvoiceDate = s.InvoiceDate,
                    TotalAmount = s.TotalAmount,
                    PaidAmount = s.PaidAmount,
                    CreditAmount = s.TotalAmount - s.PaidAmount
                }).ToList()
            })
            .FirstOrDefaultAsync();
    }

    public async Task<List<CustomerSearchResultDto>> SearchCustomersAsync(string keyword)
    {
        var searchKeyword = keyword.Trim().ToLower();

        return await _context.Customers
            .Include(c => c.AppUser)
            .Include(c => c.Vehicles)
            .Where(c =>
                c.AppUser.FullName.ToLower().Contains(searchKeyword) ||
                c.AppUser.Email.ToLower().Contains(searchKeyword) ||
                c.AppUser.PhoneNumber.ToLower().Contains(searchKeyword) ||
                c.Id.ToString().Contains(searchKeyword) ||
                c.Vehicles.Any(v => v.VehicleNumber.ToLower().Contains(searchKeyword)))
            .Select(c => new CustomerSearchResultDto
            {
                CustomerId = c.Id,
                FullName = c.AppUser.FullName,
                Email = c.AppUser.Email,
                PhoneNumber = c.AppUser.PhoneNumber,
                VehicleNumber = c.Vehicles
                    .Select(v => v.VehicleNumber)
                    .FirstOrDefault() ?? ""
            })
            .ToListAsync();
    }

    public async Task<CustomerReportDto> GetCustomerReportAsync()
    {
        var totalCustomers = await _context.Customers.CountAsync();

        var regularCustomers = await _context.Customers
            .Where(c => c.SalesInvoices.Count >= 3)
            .CountAsync();

        var highSpenders = await _context.Customers
            .Where(c => c.SalesInvoices.Sum(s => s.TotalAmount) >= 5000)
            .CountAsync();

        var pendingCreditCustomers = await _context.Customers
            .Where(c => c.SalesInvoices.Any(s => s.TotalAmount > s.PaidAmount))
            .CountAsync();

        return new CustomerReportDto
        {
            TotalCustomers = totalCustomers,
            RegularCustomers = regularCustomers,
            HighSpenders = highSpenders,
            PendingCreditCustomers = pendingCreditCustomers
        };
    }
}