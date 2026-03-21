using KT_GK.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

// Đổi từ DbContext sang IdentityDbContext
public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<NganhHoc> NganhHocs { get; set; }
    public DbSet<SinhVien> SinhViens { get; set; }
    public DbSet<HocPhan> HocPhans { get; set; }
    public DbSet<DangKy> DangKys { get; set; }
    public DbSet<ChiTietDangKy> ChiTietDangKys { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); // Bắt buộc phải có dòng này cho Identity
        modelBuilder.Entity<ChiTietDangKy>().HasKey(c => new { c.MaDK, c.MaHP });
    }
}