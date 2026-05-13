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
            .AsNoTracking()
            .Include(c => c.AppUser)
            .Include(c => c.Vehicles)
            .Include(c => c.SalesInvoices)
                .ThenInclude(s => s.Items)
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

                PurchaseHistory = c.SalesInvoices
                    .OrderByDescending(s => s.InvoiceDate)
                    .Select(s => new CustomerInvoiceHistoryDto
                    {
                        InvoiceId = s.Id,
                        InvoiceNumber = s.InvoiceNumber,
                        InvoiceDate = s.InvoiceDate,
                        TotalAmount = s.TotalAmount,
                        PaidAmount = s.PaidAmount,
                        CreditAmount = s.TotalAmount - s.PaidAmount,

                        Items = s.Items.Select(i => new CustomerInvoiceItemHistoryDto
                        {
                            PartName = i.PartName,
                            Quantity = i.Quantity,
                            UnitPrice = i.UnitPrice,
                            SubTotal = i.Quantity * i.UnitPrice
                        }).ToList()
                    }).ToList()
            })
            .FirstOrDefaultAsync();
    }

    public async Task<List<CustomerSearchResultDto>> SearchCustomersAsync(string keyword)
    {
        var searchKeyword = keyword.Trim().ToLower();

        return await _context.Customers
            .AsNoTracking()
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

        var customerReportData = await _context.Customers
            .AsNoTracking()
            .Include(c => c.AppUser)
            .Select(c => new CustomerReportItemDto
            {
                CustomerId = c.Id,
                FullName = c.AppUser.FullName,
                Email = c.AppUser.Email,
                PhoneNumber = c.AppUser.PhoneNumber,

                InvoiceCount = c.SalesInvoices.Count(),

                TotalSpent = c.SalesInvoices
                    .Sum(s => (decimal?)s.TotalAmount) ?? 0,

                PendingCreditAmount = c.SalesInvoices
                    .Where(s => s.TotalAmount > s.PaidAmount)
                    .Sum(s => (decimal?)(s.TotalAmount - s.PaidAmount)) ?? 0
            })
            .ToListAsync();

        var regularCustomerList = customerReportData
            .Where(c => c.InvoiceCount >= 3)
            .OrderByDescending(c => c.InvoiceCount)
            .ToList();

        var highSpenderList = customerReportData
            .Where(c => c.TotalSpent >= 5000)
            .OrderByDescending(c => c.TotalSpent)
            .ToList();

        var pendingCreditCustomerList = customerReportData
            .Where(c => c.PendingCreditAmount > 0)
            .OrderByDescending(c => c.PendingCreditAmount)
            .ToList();

        return new CustomerReportDto
        {
            TotalCustomers = totalCustomers,

            RegularCustomers = regularCustomerList.Count,
            HighSpenders = highSpenderList.Count,
            PendingCreditCustomers = pendingCreditCustomerList.Count,

            RegularCustomerList = regularCustomerList,
            HighSpenderList = highSpenderList,
            PendingCreditCustomerList = pendingCreditCustomerList
        };
    }
}