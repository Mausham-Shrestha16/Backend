using VehiclePartsInventory.Application.DTOs.Customers;
using VehiclePartsInventory.Application.DTOs.Reports;
using VehiclePartsInventory.Domain.Entities;

namespace VehiclePartsInventory.Application.Interfaces;

public interface ICustomerRepository
{
    Task<bool> VehicleNumberExistsAsync(string vehicleNumber);

    Task AddCustomerAsync(Customer customer);

    Task<CustomerDetailsDto?> GetCustomerDetailsAsync(int customerId);

    Task<List<CustomerSearchResultDto>> SearchCustomersAsync(string keyword);

    Task<CustomerReportDto> GetCustomerReportAsync();
}