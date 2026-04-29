using Microsoft.EntityFrameworkCore;
using VehiclePartsInventory.Domain.Entities;

namespace VehiclePartsInventory.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<AppUser> Users => Set<AppUser>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Vehicle> Vehicles => Set<Vehicle>();
    public DbSet<SalesInvoice> SalesInvoices => Set<SalesInvoice>();
    public DbSet<SalesInvoiceItem> SalesInvoiceItems => Set<SalesInvoiceItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<AppUser>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<AppUser>()
            .HasIndex(u => u.PhoneNumber)
            .IsUnique();

        modelBuilder.Entity<AppUser>()
            .Property(u => u.Role)
            .HasConversion<string>();

        modelBuilder.Entity<Customer>()
            .HasOne(c => c.AppUser)
            .WithOne(u => u.Customer)
            .HasForeignKey<Customer>(c => c.AppUserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Vehicle>()
            .HasOne(v => v.Customer)
            .WithMany(c => c.Vehicles)
            .HasForeignKey(v => v.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Vehicle>()
            .HasIndex(v => v.VehicleNumber)
            .IsUnique();

        modelBuilder.Entity<SalesInvoice>()
            .HasOne(s => s.Customer)
            .WithMany(c => c.SalesInvoices)
            .HasForeignKey(s => s.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<SalesInvoiceItem>()
            .HasOne(i => i.SalesInvoice)
            .WithMany(s => s.Items)
            .HasForeignKey(i => i.SalesInvoiceId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<SalesInvoice>()
            .Property(s => s.TotalAmount)
            .HasColumnType("numeric(18,2)");

        modelBuilder.Entity<SalesInvoice>()
            .Property(s => s.PaidAmount)
            .HasColumnType("numeric(18,2)");

        modelBuilder.Entity<SalesInvoiceItem>()
            .Property(i => i.UnitPrice)
            .HasColumnType("numeric(18,2)");
    }
}