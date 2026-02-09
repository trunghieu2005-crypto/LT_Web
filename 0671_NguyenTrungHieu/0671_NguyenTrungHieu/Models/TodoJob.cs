using System.ComponentModel.DataAnnotations;

namespace _0671_NguyenTrungHieu.Models
{
    public class TodoJob
    {
        [Required(ErrorMessage = "Vui lòng nhập vào mã công việc")]
        public int MaCongViec { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập vào tên công việc")]
        public string TenCongViec { get; set; }
        public bool HoanThanh { get; set; }
    }
}
