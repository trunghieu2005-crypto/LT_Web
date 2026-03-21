using System.ComponentModel.DataAnnotations;

namespace Bai6_DATM.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}