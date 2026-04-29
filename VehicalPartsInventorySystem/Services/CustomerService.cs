using BCrypt.Net;
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

        if (await _userRepository.EmailExistsAsync(dto.Email))
            throw new Exception("Email already exists.");

        if (await _userRepository.PhoneExistsAsync(dto.PhoneNumber))
            throw new Exception("Phone number already exists.");

        if (await _customerRepository.VehicleNumberExistsAsync(dto.Vehicle.VehicleNumber))
            throw new Exception("Vehicle number already exists.");

        var temporaryPassword = "Customer@123";

        var user = new AppUser
        {
            FullName = dto.FullName,
            Email = dto.Email.Trim().ToLower(),
            PhoneNumber = dto.PhoneNumber,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(temporaryPassword),
            Role = UserRole.Customer,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        var customer = new Customer
        {
            AppUser = user,
            Address = dto.Address,
            RegisteredAt = DateTime.UtcNow,
            Vehicles = new List<Vehicle>
            {
                new Vehicle
                {
                    VehicleNumber = dto.Vehicle.VehicleNumber,
                    VehicleType = dto.Vehicle.VehicleType,
                    Brand = dto.Vehicle.Brand,
                    Model = dto.Vehicle.Model,
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

    public async Task<List<CustomerSearchResultDto>> SearchCustomersAsync(string keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword))
            throw new Exception("Search keyword is required.");

        return await _customerRepository.SearchCustomersAsync(keyword);
    }

    public async Task<CustomerReportDto> GetCustomerReportAsync()
    {
        return await _customerRepository.GetCustomerReportAsync();
    }
}