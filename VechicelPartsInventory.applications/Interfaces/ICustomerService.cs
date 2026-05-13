using VehiclePartsInventory.Application.DTOs.Customers;
using VehiclePartsInventory.Application.DTOs.Reports;

namespace VehiclePartsInventory.Application.Interfaces;

public interface ICustomerService
{
    Task<CustomerDetailsDto> RegisterCustomerByStaffAsync(CustomerCreateDto dto);

    Task<CustomerDetailsDto?> GetCustomerDetailsAsync(int customerId);

    Task<CustomerDetailsDto?> GetMyProfileAsync(int appUserId);

    Task<CustomerDetailsDto?> UpdateMyProfileAsync(int appUserId, UpdateCustomerProfileDto dto);

    Task<CustomerDetailsDto?> AddMyVehicleAsync(int appUserId, VehicleCreateDto dto);

    Task<CustomerDetailsDto?> UpdateMyVehicleAsync(int appUserId, int vehicleId, VehicleCreateDto dto);

    Task<bool> DeleteMyVehicleAsync(int appUserId, int vehicleId);

    Task<List<CustomerInvoiceHistoryDto>> GetMyPurchaseHistoryAsync(int appUserId);

    Task<List<CustomerSearchResultDto>> SearchCustomersAsync(string keyword);

    Task<CustomerReportDto> GetCustomerReportAsync();
}