using HotelBookingSystem.Domain.Models;
using HotelBookingSystem.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystem.Infrastructure.Persistence;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    { 
    }
    public DbSet<Hotel> Hotels { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<City> Cities { get; set; }
    public DbSet<Guest> Guests { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<RoomImage> RoomImages { get; set; }
    public DbSet<HotelImage> HotelImages { get; set; }
    public DbSet<CityImage> CityImages { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<Discount> Discounts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        IgnoreComputedProperties(modelBuilder);

        SetPrecisionForFloatingPointTypes(modelBuilder);

        modelBuilder.Entity<ApplicationUser>()
            .HasOne(a => a.Guest)
            .WithOne()
            .HasForeignKey<ApplicationUser>("GuestId"); 

        SeedData(modelBuilder);

        ConfigureDeleteBehavior(modelBuilder);
    }

    private void ConfigureDeleteBehavior(ModelBuilder modelBuilder)
    {
        // Set default delete behavior to Restrict
        var cascadeDeleteFKs = modelBuilder.Model.GetEntityTypes()
            .SelectMany(t => t.GetForeignKeys())
            .Where(fk => !fk.IsOwnership && fk.DeleteBehavior != DeleteBehavior.Restrict);

        foreach (var fk in cascadeDeleteFKs)
            fk.DeleteBehavior = DeleteBehavior.Restrict;

        // Set cascade delete specifically for image entities
        modelBuilder.Entity<City>()
            .HasMany(c => c.Images)
            .WithOne()
            .HasForeignKey(ci => ci.CityId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Hotel>()
            .HasMany(h => h.Images)
            .WithOne()
            .HasForeignKey(hi => hi.HotelId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Room>()
            .HasMany(r => r.Images)
            .WithOne()
            .HasForeignKey(ri => ri.RoomId)
            .OnDelete(DeleteBehavior.Cascade);


        modelBuilder.Entity<Booking>()
            .HasMany(b => b.Rooms)
            .WithMany(r => r.Bookings)
            .UsingEntity<Dictionary<string, object>>(
                "BookingRoom",
                j => j.HasOne<Room>().WithMany().HasForeignKey("RoomsId").OnDelete(DeleteBehavior.Cascade),
                j => j.HasOne<Booking>().WithMany().HasForeignKey("BookingsId").OnDelete(DeleteBehavior.Cascade)
            );

    }

    private void SetPrecisionForFloatingPointTypes(ModelBuilder modelBuilder)
    {
        // set precision for decimal properties
        modelBuilder.Entity<Room>()
                    .Property(r => r.Price)
                    .HasPrecision(18, 2);
        modelBuilder.Entity<Booking>()
                    .Property(b => b.Price)
                    .HasPrecision(18, 2);
        modelBuilder.Entity<Discount>()
                    .Property(d => d.Percentage)
                    .HasPrecision(18, 2);

        // set precision for double properties
        modelBuilder.Entity<Hotel>()
                    .Property(h => h.Latitude)
                    .HasPrecision(8, 6);
        modelBuilder.Entity<Hotel>()
                    .Property(h => h.Longitude)
                    .HasPrecision(9, 6);
    }

    private void IgnoreComputedProperties(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Hotel>()
                    .Ignore(h => h.RoomsNumber);
        modelBuilder.Entity<Discount>()
                    .Ignore(d => d.IsActive);
        modelBuilder.Entity<Discount>()
                    .Ignore(d => d.OriginalPrice);
        modelBuilder.Entity<Discount>()
                    .Ignore(d => d.DiscountedPrice);
        modelBuilder.Entity<Guest>()
                    .Ignore(g => g.FullName);
    }

    private static void SeedData(ModelBuilder modelBuilder)
    {
        var newYorkId = new Guid("1183b59c-f7f8-4b21-b1df-5149fb57984e");
        var londonId = new Guid("1283b59c-f7f8-4b21-b1df-5149fb57984e");
        var parisId = new Guid("1383b59c-f7f8-4b21-b1df-5149fb57984e");
        var tokyoId = new Guid("1483b59c-f7f8-4b21-b1df-5149fb57984e");
        var berlinId = new Guid("1583b59c-f7f8-4b21-b1df-5149fb57984e");

        modelBuilder.Entity<City>().HasData(
            new City { Id = newYorkId, Name = "New York", Country = "USA", PostOffice = "10001", CreationDate = new DateTime(2023, 12, 14), LastModified = new DateTime(2023, 12, 14) },
            new City { Id = londonId, Name = "London", Country = "UK", PostOffice = "SW1A 1AA", CreationDate = new DateTime(2023, 12, 14), LastModified = new DateTime(2023, 12, 14) },
            new City { Id = parisId, Name = "Paris", Country = "France", PostOffice = "75001", CreationDate = new DateTime(2023, 12, 14), LastModified = new DateTime(2023, 12, 14) },
            new City { Id = tokyoId, Name = "Tokyo", Country = "Japan", PostOffice = "100-0001", CreationDate = new DateTime(2023, 12, 14), LastModified = new DateTime(2023, 12, 14) },
            new City { Id = berlinId, Name = "Berlin", Country = "Germany", PostOffice = "10115", CreationDate = new DateTime(2023, 12, 14), LastModified = new DateTime(2023, 12, 14) }
        );

        var grandHyattId = new Guid("1683b59c-f7f8-4b21-b1df-5149fb57984e");
        var theRitzId = new Guid("1783b59c-f7f8-4b21-b1df-5149fb57984e");
        var leMeridienId = new Guid("1883b59c-f7f8-4b21-b1df-5149fb57984e");
        var tokyoPalaceId = new Guid("1983b59c-f7f8-4b21-b1df-5149fb57984e");
        var berlinGrandId = new Guid("2183b59c-f7f8-4b21-b1df-5149fb57984e");

        modelBuilder.Entity<Hotel>().HasData(
            new Hotel { Id = grandHyattId, Name = "Grand Hyatt", Owner = "Hyatt Group", StarRate = 5, Street = "Times Square, New York", CityId = newYorkId, CheckInTime = new TimeOnly(14, 0), CheckOutTime = new TimeOnly(12, 0), CreationDate = new DateTime(2023, 12, 14), LastModified = new DateTime(2023, 12, 14), Latitude =12.345678, Longitude = 153.456789  },
            new Hotel { Id = theRitzId, Name = "The Ritz", Owner = "Ritz-Carlton", StarRate = 5, Street = "Piccadilly, London", CityId = londonId, CheckInTime = new TimeOnly(15, 0), CheckOutTime = new TimeOnly(11, 30), CreationDate = new DateTime(2023, 12, 14), LastModified = new DateTime(2023, 12, 14), Latitude = 13.345678, Longitude = 143.456789 },
            new Hotel { Id = leMeridienId, Name = "Le Méridien", Owner = "Marriott Group", StarRate = 4, Street = "Champs-Élysées, Paris", CityId = parisId, CheckInTime = new TimeOnly(14, 30), CheckOutTime = new TimeOnly(12, 30), CreationDate = new DateTime(2023, 12, 14), LastModified = new DateTime(2023, 12, 14), Latitude = 14.345678, Longitude = 133.456789 },
            new Hotel { Id = tokyoPalaceId, Name = "Tokyo Palace", Owner = "Palace Hotels", StarRate = 4, Street = "Chiyoda, Tokyo", CityId = tokyoId, CheckInTime = new TimeOnly(14, 0), CheckOutTime = new TimeOnly(12, 0), CreationDate = new DateTime(2023, 12, 14), LastModified = new DateTime(2023, 12, 14), Latitude = 15.345678, Longitude = 123.456789 },
            new Hotel { Id = berlinGrandId, Name = "Berlin Grand", Owner = "Grand Hotels", StarRate = 4, Street = "Mitte, Berlin", CityId = berlinId, CheckInTime = new TimeOnly(15, 0), CheckOutTime = new TimeOnly(11, 30), CreationDate = new DateTime(2023, 12, 14), LastModified = new DateTime(2023, 12, 14), Latitude = 16.345678, Longitude = 113.456789 }
        );

        var room1GrandHyattId = new Guid("2283b59c-f7f8-4b21-b1df-5149fb57984e");
        var room1TheRitzId = new Guid("2383b59c-f7f8-4b21-b1df-5149fb57984e");
        var room1LeMeridienId = new Guid("2483b59c-f7f8-4b21-b1df-5149fb57984e");
        var room1TokyoPalaceId = new Guid("2583b59c-f7f8-4b21-b1df-5149fb57984e");
        var room1BerlinGrandId = new Guid("2683b59c-f7f8-4b21-b1df-5149fb57984e");

        modelBuilder.Entity<Room>().HasData(
            new Room { Id = room1GrandHyattId, RoomNumber = 1, AdultsCapacity = 2, ChildrenCapacity = 1, Price = 300, HotelId = grandHyattId, RoomType = RoomType.Standard, CreationDate = new DateTime(2023, 12, 14), LastModified = new DateTime(2023, 12, 14) },
            new Room { Id = room1TheRitzId, RoomNumber = 1, AdultsCapacity = 3, ChildrenCapacity = 0, Price = 500, HotelId = theRitzId, RoomType = RoomType.Luxury, CreationDate = new DateTime(2023, 12, 14), LastModified = new DateTime(2023, 12, 14) },
            new Room { Id = room1LeMeridienId, RoomNumber = 1, AdultsCapacity = 2, ChildrenCapacity = 2, Price = 250, HotelId = leMeridienId, RoomType = RoomType.Boutique, CreationDate = new DateTime(2023, 12, 14), LastModified = new DateTime(2023, 12, 14) },
            new Room { Id = room1TokyoPalaceId, RoomNumber = 1, AdultsCapacity = 2, ChildrenCapacity = 1, Price = 350, HotelId = tokyoPalaceId, RoomType = RoomType.Standard, CreationDate = new DateTime(2023, 12, 14), LastModified = new DateTime(2023, 12, 14) },
            new Room { Id = room1BerlinGrandId, RoomNumber = 1, AdultsCapacity = 2, ChildrenCapacity = 0, Price = 450, HotelId = berlinGrandId, RoomType = RoomType.Boutique, CreationDate = new DateTime(2023, 12, 14), LastModified = new DateTime(2023, 12, 14) }
        );
    }

}
