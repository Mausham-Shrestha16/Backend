namespace VehiclePartsInventory.Domain.Entities;

    public class Customer
    {
        public int Id { get; set; }
        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; } = null!;

        public string Address { get; set; } = string.Empty;

        public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;

        public ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();

        public ICollection<SalesInvoice> SalesInvoices { get; set; } = new List<SalesInvoice>();

    }

