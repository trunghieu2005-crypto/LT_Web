using System.ComponentModel.DataAnnotations;

namespace KT_GK.Models
{
    public class DangKy
    {
        [Key]
        public int MaDK { get; set; }
        public DateTime NgayDK { get; set; }
        public string MaSV { get; set; }
        public SinhVien SinhVien { get; set; }
        public ICollection<ChiTietDangKy> ChiTietDangKys { get; set; }
    }
}
