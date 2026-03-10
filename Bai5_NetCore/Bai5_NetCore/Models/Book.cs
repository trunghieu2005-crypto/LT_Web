namespace Bai5_NetCore.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Book
    {
        public int BookId { get; set; }

        [Required]
        public string Title { get; set; }

        public string Author { get; set; }

        public double Price { get; set; }

        public string? Image { get; set; }

        public string? Description { get; set; }

        public int CategoryId { get; set; }

        public Category? Category { get; set; }
    }
}
