using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
namespace Web_API.Models;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public decimal Price { get; set; }
    [MaxLength(255)]
    public string? ImageName { get; set; } = string.Empty;

    public int CategoryId { get; set; }

    [JsonIgnore]
    public Category? Category { get; set; }
    public string Description { get; set; }
}
