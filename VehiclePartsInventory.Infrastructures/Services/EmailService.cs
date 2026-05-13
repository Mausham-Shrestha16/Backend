using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using VehiclePartsInventory.Application.Interfaces;

namespace VehiclePartsInventory.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly ISalesInvoiceRepository _salesInvoiceRepository;
    private readonly IPartRepository _partRepository;
    private readonly IConfiguration _config;

    public EmailService(
        ISalesInvoiceRepository salesInvoiceRepository,
        IPartRepository partRepository,
        IConfiguration config)
    {
        _salesInvoiceRepository = salesInvoiceRepository;
        _partRepository = partRepository;
        _config = config;
    }

    public async Task SendInvoiceEmailAsync(int invoiceId)
    {
        var invoice = await _salesInvoiceRepository.GetByIdAsync(invoiceId)
            ?? throw new Exception("Invoice not found.");

        var customerEmail = invoice.Customer?.AppUser?.Email;
        var customerName = invoice.Customer?.AppUser?.FullName ?? "Customer";

        if (string.IsNullOrWhiteSpace(customerEmail))
            throw new Exception("Customer email not found.");

        var itemsHtml = string.Join("", invoice.Items.Select(i =>
            $"<tr><td>{i.PartName}</td><td>{i.Quantity}</td><td>${i.UnitPrice:F2}</td><td>${i.Quantity * i.UnitPrice:F2}</td></tr>"));

        var body = $"""
            <h2>Invoice {invoice.InvoiceNumber}</h2>
            <p>Dear {customerName},</p>
            <p>Please find your invoice details below:</p>
            <table border='1' cellpadding='8' style='border-collapse:collapse;'>
              <thead>
                <tr><th>Part</th><th>Qty</th><th>Unit Price</th><th>Subtotal</th></tr>
              </thead>
              <tbody>{itemsHtml}</tbody>
            </table>
            <p><strong>Total: ${invoice.TotalAmount:F2}</strong></p>
            <p><strong>Paid: ${invoice.PaidAmount:F2}</strong></p>
            <p><strong>Balance Due: ${invoice.TotalAmount - invoice.PaidAmount:F2}</strong></p>
            <br/><p>Thank you for your business.</p>
            <p>Vehicle Parts Inventory</p>
            """;

        await SendEmailAsync(customerEmail, $"Invoice {invoice.InvoiceNumber} - Vehicle Parts", body);
    }

    public async Task SendLowStockAlertAsync()
    {
        var parts = await _partRepository.GetAllPartsAsync();
        var lowStock = parts.Where(p => p.StockQuantity <= p.ReorderLevel).ToList();

        if (lowStock.Count == 0) return;

        var rows = string.Join("", lowStock.Select(p =>
            $"<tr><td>{p.PartNumber}</td><td>{p.Name}</td><td>{p.StockQuantity}</td><td>{p.ReorderLevel}</td></tr>"));

        var body = $"""
            <h2>Low Stock Alert</h2>
            <p>The following parts are at or below reorder level:</p>
            <table border='1' cellpadding='8' style='border-collapse:collapse;'>
              <thead>
                <tr><th>Part #</th><th>Name</th><th>Stock</th><th>Reorder Level</th></tr>
              </thead>
              <tbody>{rows}</tbody>
            </table>
            <p>Please arrange for restocking.</p>
            """;

        var adminEmail = _config["Email:AdminEmail"] ?? "";
        if (!string.IsNullOrWhiteSpace(adminEmail))
            await SendEmailAsync(adminEmail, "Low Stock Alert - Vehicle Parts Inventory", body);
    }

    public async Task SendOverdueCreditRemindersAsync()
    {
        var invoices = await _salesInvoiceRepository.GetAllAsync();

        var overdueGroups = invoices
            .Where(i => i.TotalAmount > i.PaidAmount)
            .GroupBy(i => i.Customer)
            .ToList();

        foreach (var group in overdueGroups)
        {
            var customer = group.Key;
            if (customer?.AppUser == null) continue;

            var totalCredit = group.Sum(i => i.TotalAmount - i.PaidAmount);
            var invoiceList = string.Join("", group.Select(i =>
                $"<tr><td>{i.InvoiceNumber}</td><td>{i.InvoiceDate:dd MMM yyyy}</td><td>${i.TotalAmount - i.PaidAmount:F2}</td></tr>"));

            var body = $"""
                <h2>Outstanding Balance Reminder</h2>
                <p>Dear {customer.AppUser.FullName},</p>
                <p>You have outstanding balances on the following invoices:</p>
                <table border='1' cellpadding='8' style='border-collapse:collapse;'>
                  <thead>
                    <tr><th>Invoice #</th><th>Date</th><th>Balance Due</th></tr>
                  </thead>
                  <tbody>{invoiceList}</tbody>
                </table>
                <p><strong>Total Outstanding: ${totalCredit:F2}</strong></p>
                <p>Please settle your balance at your earliest convenience.</p>
                <p>Vehicle Parts Inventory</p>
                """;

            await SendEmailAsync(customer.AppUser.Email,
                "Outstanding Balance Reminder - Vehicle Parts Inventory", body);
        }
    }

    private async Task SendEmailAsync(string to, string subject, string htmlBody)
    {
        var host = _config["Email:SmtpHost"] ?? "smtp.gmail.com";
        var port = int.Parse(_config["Email:SmtpPort"] ?? "587");
        var senderEmail = _config["Email:SenderEmail"] ?? "";
        var senderPassword = _config["Email:SenderPassword"] ?? "";
        var senderName = _config["Email:SenderName"] ?? "Vehicle Parts Inventory";

        using var client = new SmtpClient(host, port)
        {
            Credentials = new NetworkCredential(senderEmail, senderPassword),
            EnableSsl = true
        };

        var message = new MailMessage
        {
            From = new MailAddress(senderEmail, senderName),
            Subject = subject,
            Body = htmlBody,
            IsBodyHtml = true
        };
        message.To.Add(to);

        await client.SendMailAsync(message);
    }
}
