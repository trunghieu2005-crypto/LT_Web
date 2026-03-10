using System.ComponentModel.DataAnnotations;

namespace Bai5_NetCore.Models
{
    public class Category
    {
        public int CategoryId { get; set; }

        [Required]
        public string CategoryName { get; set; }

        public ICollection<Book> Books { get; set; }
    }
}
