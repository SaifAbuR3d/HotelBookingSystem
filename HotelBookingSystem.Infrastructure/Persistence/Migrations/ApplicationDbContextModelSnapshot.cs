﻿// <auto-generated />
using HotelBookingSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

#nullable disable

namespace HotelBookingSystem.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("HotelBookingSystem.Domain.Models.Booking", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateOnly>("CheckInDate")
                        .HasColumnType("date");

                    b.Property<DateOnly>("CheckOutDate")
                        .HasColumnType("date");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("GuestId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("LastModified")
                        .HasColumnType("datetime2");

                    b.Property<int>("NumberOfAdults")
                        .HasColumnType("int");

                    b.Property<int>("NumberOfChildren")
                        .HasColumnType("int");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<Guid>("RoomId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("GuestId");

                    b.HasIndex("RoomId");

                    b.ToTable("Bookings", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("3683b59c-f7f8-4b21-b1df-5149fb57984e"),
                            CheckInDate = new DateOnly(2023, 3, 5),
                            CheckOutDate = new DateOnly(2023, 3, 10),
                            CreationDate = new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            GuestId = new Guid("2783b59c-f7f8-4b21-b1df-5149fb57984e"),
                            LastModified = new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            NumberOfAdults = 0,
                            NumberOfChildren = 0,
                            Price = 600m,
                            RoomId = new Guid("2683b59c-f7f8-4b21-b1df-5149fb57984e")
                        },
                        new
                        {
                            Id = new Guid("3783b59c-f7f8-4b21-b1df-5149fb57984e"),
                            CheckInDate = new DateOnly(2023, 4, 15),
                            CheckOutDate = new DateOnly(2023, 4, 20),
                            CreationDate = new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            GuestId = new Guid("2883b59c-f7f8-4b21-b1df-5149fb57984e"),
                            LastModified = new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            NumberOfAdults = 0,
                            NumberOfChildren = 0,
                            Price = 700m,
                            RoomId = new Guid("2283b59c-f7f8-4b21-b1df-5149fb57984e")
                        },
                        new
                        {
                            Id = new Guid("3883b59c-f7f8-4b21-b1df-5149fb57984e"),
                            CheckInDate = new DateOnly(2023, 5, 8),
                            CheckOutDate = new DateOnly(2023, 5, 15),
                            CreationDate = new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            GuestId = new Guid("2983b59c-f7f8-4b21-b1df-5149fb57984e"),
                            LastModified = new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            NumberOfAdults = 0,
                            NumberOfChildren = 0,
                            Price = 550m,
                            RoomId = new Guid("2483b59c-f7f8-4b21-b1df-5149fb57984e")
                        },
                        new
                        {
                            Id = new Guid("3983b59c-f7f8-4b21-b1df-5149fb57984e"),
                            CheckInDate = new DateOnly(2023, 6, 20),
                            CheckOutDate = new DateOnly(2023, 6, 25),
                            CreationDate = new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            GuestId = new Guid("3183b59c-f7f8-4b21-b1df-5149fb57984e"),
                            LastModified = new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            NumberOfAdults = 0,
                            NumberOfChildren = 0,
                            Price = 800m,
                            RoomId = new Guid("2383b59c-f7f8-4b21-b1df-5149fb57984e")
                        });
                });

            modelBuilder.Entity("HotelBookingSystem.Domain.Models.City", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("LastModified")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PostOffice")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Cities", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("1183b59c-f7f8-4b21-b1df-5149fb57984e"),
                            Country = "USA",
                            CreationDate = new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            LastModified = new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "New York",
                            PostOffice = "10001"
                        },
                        new
                        {
                            Id = new Guid("1283b59c-f7f8-4b21-b1df-5149fb57984e"),
                            Country = "UK",
                            CreationDate = new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            LastModified = new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "London",
                            PostOffice = "SW1A 1AA"
                        },
                        new
                        {
                            Id = new Guid("1383b59c-f7f8-4b21-b1df-5149fb57984e"),
                            Country = "France",
                            CreationDate = new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            LastModified = new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Paris",
                            PostOffice = "75001"
                        },
                        new
                        {
                            Id = new Guid("1483b59c-f7f8-4b21-b1df-5149fb57984e"),
                            Country = "Japan",
                            CreationDate = new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            LastModified = new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Tokyo",
                            PostOffice = "100-0001"
                        },
                        new
                        {
                            Id = new Guid("1583b59c-f7f8-4b21-b1df-5149fb57984e"),
                            Country = "Germany",
                            CreationDate = new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            LastModified = new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Berlin",
                            PostOffice = "10115"
                        });
                });

            modelBuilder.Entity("HotelBookingSystem.Domain.Models.CityImage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AlternativeText")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("CityId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("LastModified")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("CityId");

                    b.ToTable("CityImages", (string)null);
                });

            modelBuilder.Entity("HotelBookingSystem.Domain.Models.Discount", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("LastModified")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("Percentage")
                        .HasColumnType("decimal(18,2)");

                    b.Property<Guid>("RoomId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("RoomId");

                    b.ToTable("Discounts", (string)null);
                });

            modelBuilder.Entity("HotelBookingSystem.Domain.Models.Guest", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("LastModified")
                        .HasColumnType("datetime2");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Guests", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("2783b59c-f7f8-4b21-b1df-5149fb57984e"),
                            CreationDate = new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            LastModified = new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Username = "john_doe"
                        },
                        new
                        {
                            Id = new Guid("2883b59c-f7f8-4b21-b1df-5149fb57984e"),
                            CreationDate = new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            LastModified = new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Username = "jane_smith"
                        },
                        new
                        {
                            Id = new Guid("2983b59c-f7f8-4b21-b1df-5149fb57984e"),
                            CreationDate = new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            LastModified = new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Username = "alice_jones"
                        },
                        new
                        {
                            Id = new Guid("3183b59c-f7f8-4b21-b1df-5149fb57984e"),
                            CreationDate = new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            LastModified = new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Username = "bob_smith"
                        });
                });

            modelBuilder.Entity("HotelBookingSystem.Domain.Models.Hotel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<TimeOnly>("CheckInTime")
                        .HasColumnType("time");

                    b.Property<TimeOnly>("CheckOutTime")
                        .HasColumnType("time");

                    b.Property<Guid>("CityId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("LastModified")
                        .HasColumnType("datetime2");

                    b.Property<double>("Latitude")
                        .HasPrecision(8, 6)
                        .HasColumnType("float(8)");

                    b.Property<double>("Longitude")
                        .HasPrecision(9, 6)
                        .HasColumnType("float(9)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Owner")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<short>("StarRate")
                        .HasColumnType("smallint");

                    b.Property<string>("Street")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CityId");

                    b.ToTable("Hotels", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("1683b59c-f7f8-4b21-b1df-5149fb57984e"),
                            CheckInTime = new TimeOnly(14, 0, 0),
                            CheckOutTime = new TimeOnly(12, 0, 0),
                            CityId = new Guid("1183b59c-f7f8-4b21-b1df-5149fb57984e"),
                            CreationDate = new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            LastModified = new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Latitude = 12.345677999999999,
                            Longitude = 153.45678899999999,
                            Name = "Grand Hyatt",
                            Owner = "Hyatt Group",
                            StarRate = (short)5,
                            Street = "Times Square, New York"
                        },
                        new
                        {
                            Id = new Guid("1783b59c-f7f8-4b21-b1df-5149fb57984e"),
                            CheckInTime = new TimeOnly(15, 0, 0),
                            CheckOutTime = new TimeOnly(11, 30, 0),
                            CityId = new Guid("1283b59c-f7f8-4b21-b1df-5149fb57984e"),
                            CreationDate = new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            LastModified = new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Latitude = 13.345677999999999,
                            Longitude = 143.45678899999999,
                            Name = "The Ritz",
                            Owner = "Ritz-Carlton",
                            StarRate = (short)5,
                            Street = "Piccadilly, London"
                        },
                        new
                        {
                            Id = new Guid("1883b59c-f7f8-4b21-b1df-5149fb57984e"),
                            CheckInTime = new TimeOnly(14, 30, 0),
                            CheckOutTime = new TimeOnly(12, 30, 0),
                            CityId = new Guid("1383b59c-f7f8-4b21-b1df-5149fb57984e"),
                            CreationDate = new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            LastModified = new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Latitude = 14.345677999999999,
                            Longitude = 133.45678899999999,
                            Name = "Le Méridien",
                            Owner = "Marriott Group",
                            StarRate = (short)4,
                            Street = "Champs-Élysées, Paris"
                        },
                        new
                        {
                            Id = new Guid("1983b59c-f7f8-4b21-b1df-5149fb57984e"),
                            CheckInTime = new TimeOnly(14, 0, 0),
                            CheckOutTime = new TimeOnly(12, 0, 0),
                            CityId = new Guid("1483b59c-f7f8-4b21-b1df-5149fb57984e"),
                            CreationDate = new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            LastModified = new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Latitude = 15.345677999999999,
                            Longitude = 123.456789,
                            Name = "Tokyo Palace",
                            Owner = "Palace Hotels",
                            StarRate = (short)4,
                            Street = "Chiyoda, Tokyo"
                        },
                        new
                        {
                            Id = new Guid("2183b59c-f7f8-4b21-b1df-5149fb57984e"),
                            CheckInTime = new TimeOnly(15, 0, 0),
                            CheckOutTime = new TimeOnly(11, 30, 0),
                            CityId = new Guid("1583b59c-f7f8-4b21-b1df-5149fb57984e"),
                            CreationDate = new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            LastModified = new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Latitude = 16.345677999999999,
                            Longitude = 113.456789,
                            Name = "Berlin Grand",
                            Owner = "Grand Hotels",
                            StarRate = (short)4,
                            Street = "Mitte, Berlin"
                        });
                });

            modelBuilder.Entity("HotelBookingSystem.Domain.Models.HotelImage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AlternativeText")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("HotelId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("LastModified")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("HotelId");

                    b.ToTable("HotelImages", (string)null);
                });

            modelBuilder.Entity("HotelBookingSystem.Domain.Models.Review", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("GuestId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("HotelId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("LastModified")
                        .HasColumnType("datetime2");

                    b.Property<int>("Rating")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("GuestId");

                    b.HasIndex("HotelId");

                    b.ToTable("Reviews", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("3283b59c-f7f8-4b21-b1df-5149fb57984e"),
                            CreationDate = new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Wonderful experience at Grand Hyatt!",
                            GuestId = new Guid("2883b59c-f7f8-4b21-b1df-5149fb57984e"),
                            HotelId = new Guid("1683b59c-f7f8-4b21-b1df-5149fb57984e"),
                            LastModified = new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Rating = 5,
                            Title = "Excellent Stay"
                        },
                        new
                        {
                            Id = new Guid("3383b59c-f7f8-4b21-b1df-5149fb57984e"),
                            CreationDate = new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "The Ritz never disappoints.",
                            GuestId = new Guid("3183b59c-f7f8-4b21-b1df-5149fb57984e"),
                            HotelId = new Guid("1783b59c-f7f8-4b21-b1df-5149fb57984e"),
                            LastModified = new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Rating = 4,
                            Title = "Amazing Service"
                        },
                        new
                        {
                            Id = new Guid("3483b59c-f7f8-4b21-b1df-5149fb57984e"),
                            CreationDate = new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Great service and comfortable stay.",
                            GuestId = new Guid("2983b59c-f7f8-4b21-b1df-5149fb57984e"),
                            HotelId = new Guid("1883b59c-f7f8-4b21-b1df-5149fb57984e"),
                            LastModified = new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Rating = 5,
                            Title = "Wonderful Experience"
                        },
                        new
                        {
                            Id = new Guid("3583b59c-f7f8-4b21-b1df-5149fb57984e"),
                            CreationDate = new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Enjoyed my time at Berlin Grand.",
                            GuestId = new Guid("2783b59c-f7f8-4b21-b1df-5149fb57984e"),
                            HotelId = new Guid("2183b59c-f7f8-4b21-b1df-5149fb57984e"),
                            LastModified = new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Rating = 4,
                            Title = "Good Stay"
                        });
                });

            modelBuilder.Entity("HotelBookingSystem.Domain.Models.Room", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("AdultsCapacity")
                        .HasColumnType("int");

                    b.Property<int>("ChildrenCapacity")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("HotelId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("LastModified")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("Price")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("RoomNumber")
                        .HasColumnType("int");

                    b.Property<int>("RoomType")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("HotelId");

                    b.ToTable("Rooms", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("2283b59c-f7f8-4b21-b1df-5149fb57984e"),
                            AdultsCapacity = 2,
                            ChildrenCapacity = 1,
                            CreationDate = new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            HotelId = new Guid("1683b59c-f7f8-4b21-b1df-5149fb57984e"),
                            LastModified = new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Price = 300m,
                            RoomNumber = 1,
                            RoomType = 0
                        },
                        new
                        {
                            Id = new Guid("2383b59c-f7f8-4b21-b1df-5149fb57984e"),
                            AdultsCapacity = 3,
                            ChildrenCapacity = 0,
                            CreationDate = new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            HotelId = new Guid("1783b59c-f7f8-4b21-b1df-5149fb57984e"),
                            LastModified = new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Price = 500m,
                            RoomNumber = 1,
                            RoomType = 1
                        },
                        new
                        {
                            Id = new Guid("2483b59c-f7f8-4b21-b1df-5149fb57984e"),
                            AdultsCapacity = 2,
                            ChildrenCapacity = 2,
                            CreationDate = new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            HotelId = new Guid("1883b59c-f7f8-4b21-b1df-5149fb57984e"),
                            LastModified = new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Price = 250m,
                            RoomNumber = 1,
                            RoomType = 3
                        },
                        new
                        {
                            Id = new Guid("2583b59c-f7f8-4b21-b1df-5149fb57984e"),
                            AdultsCapacity = 2,
                            ChildrenCapacity = 1,
                            CreationDate = new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            HotelId = new Guid("1983b59c-f7f8-4b21-b1df-5149fb57984e"),
                            LastModified = new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Price = 350m,
                            RoomNumber = 1,
                            RoomType = 0
                        },
                        new
                        {
                            Id = new Guid("2683b59c-f7f8-4b21-b1df-5149fb57984e"),
                            AdultsCapacity = 2,
                            ChildrenCapacity = 0,
                            CreationDate = new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            HotelId = new Guid("2183b59c-f7f8-4b21-b1df-5149fb57984e"),
                            LastModified = new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Price = 450m,
                            RoomNumber = 1,
                            RoomType = 3
                        });
                });

            modelBuilder.Entity("HotelBookingSystem.Domain.Models.RoomImage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AlternativeText")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("LastModified")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("RoomId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("RoomId");

                    b.ToTable("RoomImages", (string)null);
                });

            modelBuilder.Entity("HotelBookingSystem.Domain.Models.Booking", b =>
                {
                    b.HasOne("HotelBookingSystem.Domain.Models.Guest", "Guest")
                        .WithMany("Bookings")
                        .HasForeignKey("GuestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HotelBookingSystem.Domain.Models.Room", "Room")
                        .WithMany("Bookings")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Guest");

                    b.Navigation("Room");
                });

            modelBuilder.Entity("HotelBookingSystem.Domain.Models.CityImage", b =>
                {
                    b.HasOne("HotelBookingSystem.Domain.Models.City", null)
                        .WithMany("Images")
                        .HasForeignKey("CityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("HotelBookingSystem.Domain.Models.Discount", b =>
                {
                    b.HasOne("HotelBookingSystem.Domain.Models.Room", "Room")
                        .WithMany("Discounts")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Room");
                });

            modelBuilder.Entity("HotelBookingSystem.Domain.Models.Hotel", b =>
                {
                    b.HasOne("HotelBookingSystem.Domain.Models.City", "City")
                        .WithMany("Hotels")
                        .HasForeignKey("CityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("City");
                });

            modelBuilder.Entity("HotelBookingSystem.Domain.Models.HotelImage", b =>
                {
                    b.HasOne("HotelBookingSystem.Domain.Models.Hotel", null)
                        .WithMany("Images")
                        .HasForeignKey("HotelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("HotelBookingSystem.Domain.Models.Review", b =>
                {
                    b.HasOne("HotelBookingSystem.Domain.Models.Guest", "Guest")
                        .WithMany()
                        .HasForeignKey("GuestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HotelBookingSystem.Domain.Models.Hotel", "Hotel")
                        .WithMany("Reviews")
                        .HasForeignKey("HotelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Guest");

                    b.Navigation("Hotel");
                });

            modelBuilder.Entity("HotelBookingSystem.Domain.Models.Room", b =>
                {
                    b.HasOne("HotelBookingSystem.Domain.Models.Hotel", "Hotel")
                        .WithMany("Rooms")
                        .HasForeignKey("HotelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Hotel");
                });

            modelBuilder.Entity("HotelBookingSystem.Domain.Models.RoomImage", b =>
                {
                    b.HasOne("HotelBookingSystem.Domain.Models.Room", null)
                        .WithMany("Images")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("HotelBookingSystem.Domain.Models.City", b =>
                {
                    b.Navigation("Hotels");

                    b.Navigation("Images");
                });

            modelBuilder.Entity("HotelBookingSystem.Domain.Models.Guest", b =>
                {
                    b.Navigation("Bookings");
                });

            modelBuilder.Entity("HotelBookingSystem.Domain.Models.Hotel", b =>
                {
                    b.Navigation("Images");

                    b.Navigation("Reviews");

                    b.Navigation("Rooms");
                });

            modelBuilder.Entity("HotelBookingSystem.Domain.Models.Room", b =>
                {
                    b.Navigation("Bookings");

                    b.Navigation("Discounts");

                    b.Navigation("Images");
                });
#pragma warning restore 612, 618
        }
    }
}
