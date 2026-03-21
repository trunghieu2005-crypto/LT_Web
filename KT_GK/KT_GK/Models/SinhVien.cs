using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KT_GK.Models
{
    public class SinhVien
    {
        [Key]
        [StringLength(10)]
        public string MaSV { get; set; }
        [Required]
        [StringLength(50)]
        public string HoTen { get; set; }
        [StringLength(5)]
        public string GioiTinh { get; set; }
        public DateTime NgaySinh { get; set; }
        public string? Hinh { get; set; }
        [Required]
        [StringLength(4)]
        public string MaNganh { get; set; }

        [ForeignKey("MaNganh")]
        public virtual NganhHoc? NganhHoc { get; set; }
    }
}
