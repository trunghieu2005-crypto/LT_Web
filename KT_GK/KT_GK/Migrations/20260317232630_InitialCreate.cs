using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KT_GK.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HocPhans",
                columns: table => new
                {
                    MaHP = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false),
                    TenHP = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    SoTinChi = table.Column<int>(type: "int", nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HocPhans", x => x.MaHP);
                });

            migrationBuilder.CreateTable(
                name: "NganhHocs",
                columns: table => new
                {
                    MaNganh = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    TenNganh = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NganhHocs", x => x.MaNganh);
                });

            migrationBuilder.CreateTable(
                name: "SinhViens",
                columns: table => new
                {
                    MaSV = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    HoTen = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    GioiTinh = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    NgaySinh = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Hinh = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaNganh = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NganhHocMaNganh = table.Column<string>(type: "nvarchar(4)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SinhViens", x => x.MaSV);
                    table.ForeignKey(
                        name: "FK_SinhViens_NganhHocs_NganhHocMaNganh",
                        column: x => x.NganhHocMaNganh,
                        principalTable: "NganhHocs",
                        principalColumn: "MaNganh");
                });

            migrationBuilder.CreateTable(
                name: "DangKys",
                columns: table => new
                {
                    MaDK = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NgayDK = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MaSV = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SinhVienMaSV = table.Column<string>(type: "nvarchar(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DangKys", x => x.MaDK);
                    table.ForeignKey(
                        name: "FK_DangKys_SinhViens_SinhVienMaSV",
                        column: x => x.SinhVienMaSV,
                        principalTable: "SinhViens",
                        principalColumn: "MaSV",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChiTietDangKys",
                columns: table => new
                {
                    MaDK = table.Column<int>(type: "int", nullable: false),
                    MaHP = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DangKyMaDK = table.Column<int>(type: "int", nullable: false),
                    HocPhanMaHP = table.Column<string>(type: "nvarchar(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietDangKys", x => new { x.MaDK, x.MaHP });
                    table.ForeignKey(
                        name: "FK_ChiTietDangKys_DangKys_DangKyMaDK",
                        column: x => x.DangKyMaDK,
                        principalTable: "DangKys",
                        principalColumn: "MaDK",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChiTietDangKys_HocPhans_HocPhanMaHP",
                        column: x => x.HocPhanMaHP,
                        principalTable: "HocPhans",
                        principalColumn: "MaHP",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietDangKys_DangKyMaDK",
                table: "ChiTietDangKys",
                column: "DangKyMaDK");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietDangKys_HocPhanMaHP",
                table: "ChiTietDangKys",
                column: "HocPhanMaHP");

            migrationBuilder.CreateIndex(
                name: "IX_DangKys_SinhVienMaSV",
                table: "DangKys",
                column: "SinhVienMaSV");

            migrationBuilder.CreateIndex(
                name: "IX_SinhViens_NganhHocMaNganh",
                table: "SinhViens",
                column: "NganhHocMaNganh");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChiTietDangKys");

            migrationBuilder.DropTable(
                name: "DangKys");

            migrationBuilder.DropTable(
                name: "HocPhans");

            migrationBuilder.DropTable(
                name: "SinhViens");

            migrationBuilder.DropTable(
                name: "NganhHocs");
        }
    }
}
