using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HongTraStore.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SanPhams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TenSanPham = table.Column<string>(type: "TEXT", nullable: false),
                    DanhMuc = table.Column<string>(type: "TEXT", nullable: false),
                    Gia = table.Column<decimal>(type: "TEXT", nullable: false),
                    SoLuongTon = table.Column<int>(type: "INTEGER", nullable: false),
                    MoTa = table.Column<string>(type: "TEXT", nullable: false),
                    HinhAnh = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SanPhams", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SanPhams");
        }
    }
}
