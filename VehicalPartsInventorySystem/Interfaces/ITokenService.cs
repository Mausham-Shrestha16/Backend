
using VehiclePartsInventory.Domain.Entities;
namespace VehicalPartsInventorySystem.Interfaces;

    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
