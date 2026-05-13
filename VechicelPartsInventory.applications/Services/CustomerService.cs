using VehiclePartsInventory.Application.DTOs.Customers;
using VehiclePartsInventory.Application.DTOs.Reports;
using VehiclePartsInventory.Application.Interfaces;
using VehiclePartsInventory.Domain.Entities;
using VehiclePartsInventory.Domain.Enums;

namespace VehiclePartsInventory.Application.Services;

public class CustomerService : ICustomerService
{
    private readonly IUserRepository _userRepository;
    private readonly ICustomerRepository _customerRepository;

    public CustomerService(
        IUserRepository userRepository,
        ICustomerRepository customerRepository)
    {
        _userRepository = userRepository;
        _customerRepository = customerRepository;
    }

    public async Task<CustomerDetailsDto> RegisterCustomerByStaffAsync(CustomerCreateDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.FullName))
            throw new Exception("Full name is required.");

        if (string.IsNullOrWhiteSpace(dto.Email))
            throw new Exception("Email is required.");

        if (string.IsNullOrWhiteSpace(dto.PhoneNumber))
            throw new Exception("Phone number is required.");

        if (dto.Vehicle == null)
            throw new Exception("Vehicle details are required.");

        if (string.IsNullOrWhiteSpace(dto.Vehicle.VehicleNumber))
            throw new Exception("Vehicle number is required.");

        if (await _userRepository.EmailExistsAsync(dto.Email))
            throw new Exception("Email already exists.");

        if (await _userRepository.PhoneExistsAsync(dto.PhoneNumber))
            throw new Exception("Phone number already exists.");

        if (await _customerRepository.VehicleNumberExistsAsync(dto.Vehicle.VehicleNumber))
            throw new Exception("Vehicle number already exists.");

        var temporaryPassword = "Customer@123";

        var user = new AppUser
        {
            FullName = dto.FullName.Trim(),
            Email = dto.Email.Trim().ToLower(),
            PhoneNumber = dto.PhoneNumber.Trim(),
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(temporaryPassword),
            Role = UserRole.Customer,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        var customer = new Customer
        {
            AppUser = user,
            Address = dto.Address?.Trim() ?? string.Empty,
            RegisteredAt = DateTime.UtcNow,
            Vehicles = new List<Vehicle>
            {
                new Vehicle
                {
                    VehicleNumber = dto.Vehicle.VehicleNumber.Trim(),
                    VehicleType = dto.Vehicle.VehicleType.Trim(),
                    Brand = dto.Vehicle.Brand.Trim(),
                    Model = dto.Vehicle.Model.Trim(),
                    Year = dto.Vehicle.Year,
                    CreatedAt = DateTime.UtcNow
                }
            }
        };

        await _customerRepository.AddCustomerAsync(customer);

        var result = await _customerRepository.GetCustomerDetailsAsync(customer.Id);

        if (result == null)
            throw new Exception("Customer registration failed.");

        return result;
    }

    public async Task<CustomerDetailsDto?> GetCustomerDetailsAsync(int customerId)
    {
        return await _customerRepository.GetCustomerDetailsAsync(customerId);
    }

    public async Task<CustomerDetailsDto?> GetMyProfileAsync(int appUserId)
    {
        return await _customerRepository.GetCustomerDetailsByUserIdAsync(appUserId);
    }

    public async Task<CustomerDetailsDto?> UpdateMyProfileAsync(int appUserId, UpdateCustomerProfileDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.FullName))
            throw new Exception("Full name is required.");

        if (string.IsNullOrWhiteSpace(dto.PhoneNumber))
            throw new Exception("Phone number is required.");

        return await _customerRepository.UpdateCustomerProfileByUserIdAsync(appUserId, dto);
    }

    public async Task<CustomerDetailsDto?> AddMyVehicleAsync(int appUserId, VehicleCreateDto dto)
    {
        if (dto == null)
            throw new Exception("Vehicle details are required.");

        if (string.IsNullOrWhiteSpace(dto.VehicleNumber))
            throw new Exception("Vehicle number is required.");

        if (string.IsNullOrWhiteSpace(dto.VehicleType))
            throw new Exception("Vehicle type is required.");

        if (string.IsNullOrWhiteSpace(dto.Brand))
            throw new Exception("Vehicle brand is required.");

        if (string.IsNullOrWhiteSpace(dto.Model))
            throw new Exception("Vehicle model is required.");

        if (await _customerRepository.VehicleNumberExistsAsync(dto.VehicleNumber))
            throw new Exception("Vehicle number already exists.");

        return await _customerRepository.AddVehicleByUserIdAsync(appUserId, dto);
    }

    public async Task<CustomerDetailsDto?> UpdateMyVehicleAsync(int appUserId, int vehicleId, VehicleCreateDto dto)
    {
        if (dto == null)
            throw new Exception("Vehicle details are required.");

        if (string.IsNullOrWhiteSpace(dto.VehicleNumber))
            throw new Exception("Vehicle number is required.");

        if (string.IsNullOrWhiteSpace(dto.VehicleType))
            throw new Exception("Vehicle type is required.");

        if (string.IsNullOrWhiteSpace(dto.Brand))
            throw new Exception("Vehicle brand is required.");

        if (string.IsNullOrWhiteSpace(dto.Model))
            throw new Exception("Vehicle model is required.");

        if (await _customerRepository.VehicleNumberExistsForOtherVehicleAsync(dto.VehicleNumber, vehicleId))
            throw new Exception("Vehicle number already exists.");

        return await _customerRepository.UpdateVehicleByUserIdAsync(appUserId, vehicleId, dto);
    }

    public async Task<bool> DeleteMyVehicleAsync(int appUserId, int vehicleId)
    {
        return await _customerRepository.DeleteVehicleByUserIdAsync(appUserId, vehicleId);
    }

    public async Task<List<CustomerInvoiceHistoryDto>> GetMyPurchaseHistoryAsync(int appUserId)
    {
        var customer = await _customerRepository.GetCustomerDetailsByUserIdAsync(appUserId);

        if (customer == null)
            return new List<CustomerInvoiceHistoryDto>();

        return customer.PurchaseHistory;
    }

    public async Task<List<CustomerSearchResultDto>> SearchCustomersAsync(string keyword)
    {
        return await _customerRepository.SearchCustomersAsync(keyword ?? string.Empty);
    }

    public async Task<CustomerReportDto> GetCustomerReportAsync()
    {
        return await _customerRepository.GetCustomerReportAsync();
    }
}