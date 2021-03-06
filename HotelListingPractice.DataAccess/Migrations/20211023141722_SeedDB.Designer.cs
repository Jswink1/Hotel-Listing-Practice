// <auto-generated />
using HotelListingPractice.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace HotelListingPractice.DataAccess.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20211023141722_SeedDB")]
    partial class SeedDB
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.11")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("HotelListingPractice.DataAccess.Data.Country", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ShortName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Coutries");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "America",
                            ShortName = "AM"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Jamaica",
                            ShortName = "JM"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Mexico",
                            ShortName = "ME"
                        });
                });

            modelBuilder.Entity("HotelListingPractice.DataAccess.Data.Hotel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CountryId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Rating")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("CountryId");

                    b.ToTable("Hotels");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Address = "123 Somewhere St",
                            CountryId = 1,
                            Name = "Sandals Resort and Spa",
                            Rating = 4.5
                        },
                        new
                        {
                            Id = 2,
                            Address = "555 Cleveland Ave",
                            CountryId = 2,
                            Name = "Comfort Suites",
                            Rating = 4.0
                        },
                        new
                        {
                            Id = 3,
                            Address = "9000 Royal Ave",
                            CountryId = 3,
                            Name = "Grand Palladium",
                            Rating = 5.0
                        });
                });

            modelBuilder.Entity("HotelListingPractice.DataAccess.Data.Hotel", b =>
                {
                    b.HasOne("HotelListingPractice.DataAccess.Data.Country", "Country")
                        .WithMany("Hotels")
                        .HasForeignKey("CountryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Country");
                });

            modelBuilder.Entity("HotelListingPractice.DataAccess.Data.Country", b =>
                {
                    b.Navigation("Hotels");
                });
#pragma warning restore 612, 618
        }
    }
}
