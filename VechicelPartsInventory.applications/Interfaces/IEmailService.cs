namespace VehiclePartsInventory.Application.Interfaces;

public interface IEmailService
{
    Task SendInvoiceEmailAsync(int invoiceId);
    Task SendLowStockAlertAsync();
    Task SendOverdueCreditRemindersAsync();
}
