using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace KT_GK.Models
{
    public class NganhHoc
    {
        [Key]
        [StringLength(4)]
        public string MaNganh { get; set; }

        [Required]
        [StringLength(30)]
        public string TenNganh { get; set; }

        public ICollection<SinhVien> SinhViens { get; set; } = new List<SinhVien>();
    }
}
