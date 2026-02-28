using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangePharmacistIdToIntInStockAdjustmentAndDispense : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Pharmacist",
                table: "StockAdjustments");

            migrationBuilder.DropColumn(
                name: "Pharmacist",
                table: "Dispenses");

            migrationBuilder.AddColumn<int>(
                name: "PharmacistId",
                table: "StockAdjustments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PharmacistId",
                table: "Dispenses",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PharmacistId",
                table: "StockAdjustments");

            migrationBuilder.DropColumn(
                name: "PharmacistId",
                table: "Dispenses");

            migrationBuilder.AddColumn<string>(
                name: "Pharmacist",
                table: "StockAdjustments",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Pharmacist",
                table: "Dispenses",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }
    }
}
