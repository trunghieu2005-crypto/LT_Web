using Bai6_DATM.Models;
using Microsoft.EntityFrameworkCore;

namespace Bai6_DATM.Repository
{
    public class EFCategoryRepository : ICategoryRepository
    {
        
        
            private readonly ApplicationDbContext _context;

            public EFCategoryRepository(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<IEnumerable<Category>> GetAllAsync()
            {
                return await _context.Categories.ToListAsync();
            }

            // Làm tương tự như Product

            public async Task<Category> GetByIdAsync(int id)
            {
                return await _context.Categories
                    .SingleOrDefaultAsync(x => x.Id == id);
            }

            public async Task AddAsync(Category category)
            {
                _context.Categories.Add(category);
                await _context.SaveChangesAsync();
            }

            public async Task UpdateAsync(Category category)
            {
                _context.Categories.Update(category);
                await _context.SaveChangesAsync();
            }

            public async Task DeleteAsync(int id)
            {
                var category = await _context.Categories.FindAsync(id);
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
    }
}
