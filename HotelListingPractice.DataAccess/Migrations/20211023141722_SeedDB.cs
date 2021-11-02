using Microsoft.EntityFrameworkCore.Migrations;

namespace HotelListingPractice.DataAccess.Migrations
{
    public partial class SeedDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Coutries",
                columns: new[] { "Id", "Name", "ShortName" },
                values: new object[] { 1, "America", "AM" });

            migrationBuilder.InsertData(
                table: "Coutries",
                columns: new[] { "Id", "Name", "ShortName" },
                values: new object[] { 2, "Jamaica", "JM" });

            migrationBuilder.InsertData(
                table: "Coutries",
                columns: new[] { "Id", "Name", "ShortName" },
                values: new object[] { 3, "Mexico", "ME" });

            migrationBuilder.InsertData(
                table: "Hotels",
                columns: new[] { "Id", "Address", "CountryId", "Name", "Rating" },
                values: new object[] { 1, "123 Somewhere St", 1, "Sandals Resort and Spa", 4.5 });

            migrationBuilder.InsertData(
                table: "Hotels",
                columns: new[] { "Id", "Address", "CountryId", "Name", "Rating" },
                values: new object[] { 2, "555 Cleveland Ave", 2, "Comfort Suites", 4.0 });

            migrationBuilder.InsertData(
                table: "Hotels",
                columns: new[] { "Id", "Address", "CountryId", "Name", "Rating" },
                values: new object[] { 3, "9000 Royal Ave", 3, "Grand Palladium", 5.0 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Coutries",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Coutries",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Coutries",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
