using VehiclePartsInventory.Domain.Enums;

namespace VehiclePartsInventory.Domain.Entities
{
    public class AppUser
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;

        public UserRole Role { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Customer? Customer { get; set; }
    }
}
