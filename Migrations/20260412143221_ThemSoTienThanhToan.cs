using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HongTraStore.Migrations
{
    /// <inheritdoc />
    public partial class ThemSoTienThanhToan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "SoTienThanhToan",
                table: "DonHangs",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "ThoiGianThanhToan",
                table: "DonHangs",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SoTienThanhToan",
                table: "DonHangs");

            migrationBuilder.DropColumn(
                name: "ThoiGianThanhToan",
                table: "DonHangs");
        }
    }
}
