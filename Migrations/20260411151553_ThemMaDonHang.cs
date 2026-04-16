using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HongTraStore.Migrations
{
    /// <inheritdoc />
    public partial class ThemMaDonHang : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MaDonHang",
                table: "DonHangs",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaDonHang",
                table: "DonHangs");
        }
    }
}
