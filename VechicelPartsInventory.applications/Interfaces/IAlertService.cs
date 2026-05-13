using VehiclePartsInventory.Application.DTOs.Alerts;

namespace VehiclePartsInventory.Application.Interfaces;

public interface IAlertService
{
    Task<LowStockAlertDto> GetLowStockPartsAsync();
    Task<OverdueCreditAlertDto> GetOverdueCreditCustomersAsync();
    Task SendLowStockEmailAlertAsync();
    Task SendOverdueCreditEmailAlertAsync();
}
