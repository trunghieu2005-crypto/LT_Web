using Bai5_NetCore.Models;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
 : base(options)
    {
    }

    public DbSet<Book> Books { get; set; }

    public DbSet<Category> Categories { get; set; }
}