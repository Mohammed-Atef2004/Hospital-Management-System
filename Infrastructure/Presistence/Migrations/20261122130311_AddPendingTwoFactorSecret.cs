using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Presistence.Migrations
{
    /// <inheritdoc />
    public partial class AddPendingTwoFactorSecret : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PendingTwoFactorSecret",
                table: "DomainUsers",
                type: "nvarchar(1024)",
                maxLength: 1024,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PendingTwoFactorSecret",
                table: "DomainUsers");
        }
    }
}
