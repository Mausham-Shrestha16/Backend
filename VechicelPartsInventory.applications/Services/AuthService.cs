using BCrypt.Net;
using VehicalPartsInventorySystem.Application.DTOs.Auth;
using VehicalPartsInventorySystem.Interfaces;
using VehiclePartsInventory.Application.DTOs.Auth;
using VehiclePartsInventory.Application.Interfaces;
using VehiclePartsInventory.Domain.Entities;
using VehiclePartsInventory.Domain.Enums;

namespace VehiclePartsInventory.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly ITokenService _tokenService;

    public AuthService(
        IUserRepository userRepository,
        ICustomerRepository customerRepository,
        ITokenService tokenService)
    {
        _userRepository = userRepository;
        _customerRepository = customerRepository;
        _tokenService = tokenService;
    }

    public async Task<AuthResponseDto> RegisterCustomerAsync(RegisterCustomerRequestDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.FullName))
            throw new Exception("Full name is required.");

        if (string.IsNullOrWhiteSpace(dto.Email))
            throw new Exception("Email is required.");

        if (string.IsNullOrWhiteSpace(dto.Password))
            throw new Exception("Password is required.");

        if (await _userRepository.EmailExistsAsync(dto.Email))
            throw new Exception("Email already exists.");

        if (await _userRepository.PhoneExistsAsync(dto.PhoneNumber))
            throw new Exception("Phone number already exists.");

        if (await _customerRepository.VehicleNumberExistsAsync(dto.Vehicle.VehicleNumber))
            throw new Exception("Vehicle number already exists.");

        var user = new AppUser
        {
            FullName = dto.FullName,
            Email = dto.Email.Trim().ToLower(),
            PhoneNumber = dto.PhoneNumber,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
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

        var token = _tokenService.CreateToken(user);

        return new AuthResponseDto
        {
            Token = token,
            FullName = user.FullName,
            Email = user.Email,
            Role = user.Role.ToString()
        };
    }

    public async Task<AuthResponseDto> LoginAsync(LoginRequestDto dto)
    {
        var email = dto.Email.Trim().ToLower();

        var user = await _userRepository.GetByEmailAsync(email);

        if (user == null)
            throw new Exception("Invalid email or password.");

        if (!user.IsActive)
            throw new Exception("Your account is inactive. Please contact admin.");

        var isPasswordValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);

        if (!isPasswordValid)
            throw new Exception("Invalid email or password.");

        var token = _tokenService.CreateToken(user);

        return new AuthResponseDto
        {
            Token = token,
            FullName = user.FullName,
            Email = user.Email,
            Role = user.Role.ToString()
        };
    }
}