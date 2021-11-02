using Microsoft.EntityFrameworkCore.Migrations;

namespace HotelListingPractice.DataAccess.Migrations
{
    public partial class RenameCountriesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Hotels_Coutries_CountryId",
                table: "Hotels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Coutries",
                table: "Coutries");

            migrationBuilder.RenameTable(
                name: "Coutries",
                newName: "Countries");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Countries",
                table: "Countries",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Hotels_Countries_CountryId",
                table: "Hotels",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Hotels_Countries_CountryId",
                table: "Hotels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Countries",
                table: "Countries");

            migrationBuilder.RenameTable(
                name: "Countries",
                newName: "Coutries");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Coutries",
                table: "Coutries",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Hotels_Coutries_CountryId",
                table: "Hotels",
                column: "CountryId",
                principalTable: "Coutries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
