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
    public DbSet<Part> Parts => Set<Part>();
    public DbSet<Vendor> Vendors => Set<Vendor>();
    public DbSet<PurchaseInvoice> PurchaseInvoices => Set<PurchaseInvoice>();
    public DbSet<PurchaseInvoiceItem> PurchaseInvoiceItems => Set<PurchaseInvoiceItem>();

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

        modelBuilder.Entity<Part>()
            .HasIndex(p => p.PartNumber)
            .IsUnique();

        modelBuilder.Entity<Part>()
            .Property(p => p.UnitPrice)
            .HasColumnType("numeric(18,2)");

        modelBuilder.Entity<Vendor>()
            .HasIndex(v => v.Email)
            .IsUnique();

        modelBuilder.Entity<PurchaseInvoice>()
            .HasOne(pi => pi.Vendor)
            .WithMany(v => v.PurchaseInvoices)
            .HasForeignKey(pi => pi.VendorId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<PurchaseInvoice>()
            .Property(pi => pi.TotalAmount)
            .HasColumnType("numeric(18,2)");

        modelBuilder.Entity<PurchaseInvoiceItem>()
            .HasOne(pii => pii.PurchaseInvoice)
            .WithMany(pi => pi.Items)
            .HasForeignKey(pii => pii.PurchaseInvoiceId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<PurchaseInvoiceItem>()
            .HasOne(pii => pii.Part)
            .WithMany(p => p.PurchaseInvoiceItems)
            .HasForeignKey(pii => pii.PartId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<PurchaseInvoiceItem>()
            .Property(pii => pii.UnitPrice)
            .HasColumnType("numeric(18,2)");
    }
}