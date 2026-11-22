using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Presistence.Migrations
{
    /// <inheritdoc />
    public partial class LinkDomainUsersToStaff : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StaffId",
                table: "DomainUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DomainUsers_StaffId",
                table: "DomainUsers",
                column: "StaffId",
                unique: true,
                filter: "[StaffId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_DomainUsers_Staff_StaffId",
                table: "DomainUsers",
                column: "StaffId",
                principalTable: "Staff",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DomainUsers_Staff_StaffId",
                table: "DomainUsers");

            migrationBuilder.DropIndex(
                name: "IX_DomainUsers_StaffId",
                table: "DomainUsers");

            migrationBuilder.DropColumn(
                name: "StaffId",
                table: "DomainUsers");
        }
    }
}
