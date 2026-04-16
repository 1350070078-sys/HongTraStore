using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HongTraStore.Migrations
{
    /// <inheritdoc />
    public partial class ThemThanhToan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhuongThucThanhToan",
                table: "DonHangs",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TrangThaiThanhToan",
                table: "DonHangs",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhuongThucThanhToan",
                table: "DonHangs");

            migrationBuilder.DropColumn(
                name: "TrangThaiThanhToan",
                table: "DonHangs");
        }
    }
}
