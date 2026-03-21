using System.ComponentModel.DataAnnotations;

namespace KT_GK.Models
{
    public class HocPhan
    {
        [Key]
        [StringLength(6)]
        public string MaHP { get; set; }
        [Required]
        [StringLength(30)]
        public string TenHP { get; set; }
        public int SoTinChi { get; set; }
        // Thêm trường Số lượng để làm Câu 6
        public int SoLuong { get; set; }
    }
}
