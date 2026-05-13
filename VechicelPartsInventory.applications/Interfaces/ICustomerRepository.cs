using VehiclePartsInventory.Application.DTOs.Customers;
using VehiclePartsInventory.Application.DTOs.Reports;
using VehiclePartsInventory.Domain.Entities;

namespace VehiclePartsInventory.Application.Interfaces;

public interface ICustomerRepository
{
    Task<bool> VehicleNumberExistsAsync(string vehicleNumber);

    Task<bool> VehicleNumberExistsForOtherVehicleAsync(string vehicleNumber, int vehicleId);

    Task AddCustomerAsync(Customer customer);

    Task<CustomerDetailsDto?> GetCustomerDetailsAsync(int customerId);

    Task<CustomerDetailsDto?> GetCustomerDetailsByUserIdAsync(int appUserId);

    Task<CustomerDetailsDto?> UpdateCustomerProfileByUserIdAsync(int appUserId, UpdateCustomerProfileDto dto);

    Task<CustomerDetailsDto?> AddVehicleByUserIdAsync(int appUserId, VehicleCreateDto dto);

    Task<CustomerDetailsDto?> UpdateVehicleByUserIdAsync(int appUserId, int vehicleId, VehicleCreateDto dto);

    Task<bool> DeleteVehicleByUserIdAsync(int appUserId, int vehicleId);

    Task<List<CustomerSearchResultDto>> SearchCustomersAsync(string keyword);

    Task<CustomerReportDto> GetCustomerReportAsync();
}