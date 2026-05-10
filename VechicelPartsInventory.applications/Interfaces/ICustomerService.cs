using VehiclePartsInventory.Application.DTOs.Customers;
using VehiclePartsInventory.Application.DTOs.Reports;

namespace VehiclePartsInventory.Application.Interfaces;

public interface ICustomerService
{
    Task<CustomerDetailsDto> RegisterCustomerByStaffAsync(CustomerCreateDto dto);

    Task<CustomerDetailsDto?> GetCustomerDetailsAsync(int customerId);

    Task<List<CustomerSearchResultDto>> SearchCustomersAsync(string keyword);

    Task<CustomerReportDto> GetCustomerReportAsync();
}