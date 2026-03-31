using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_API.Data;
using Web_API.Models;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace Web_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookAPIController : ControllerBase
{
    private readonly BookDbContext _context;
    private readonly string _imageFolder;

    public BookAPIController(BookDbContext context)
    {
        _context = context;
        _imageFolder = Path.Combine(Directory.GetCurrentDirectory(), "Content", "ImageBooks");
        if (!Directory.Exists(_imageFolder))
            Directory.CreateDirectory(_imageFolder);
    }

    // GET: api/BookAPI?search=term
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Book>>> Get([FromQuery] string? search)
    {
        try
        {
            var query = _context.Books.Include(b => b.Category).AsQueryable();
            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(b => b.Title.Contains(search));

            var list = await query.ToListAsync();
            return Ok(list);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    // GET: api/BookAPI/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Book>> Get(int id)
    {
        try
        {
            var book = await _context.Books.Include(b => b.Category).FirstOrDefaultAsync(b => b.Id == id);
            if (book == null) return NotFound();
            return Ok(book);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    // POST: api/BookAPI
    // Content-Type: multipart/form-data
    [HttpPost]
    public async Task<IActionResult> Post([FromForm] Book book, IFormFile? imageFile)
    {
        ModelState.Clear();
        try
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                var ext = Path.GetExtension(imageFile.FileName);
                var fileName = $"{Guid.NewGuid()}{ext}";
                var filePath = Path.Combine(_imageFolder, fileName);
                await using (var stream = System.IO.File.Create(filePath))
                {
                    await imageFile.CopyToAsync(stream);
                }
                book.ImageName = fileName;
            }

            book.Category = null;//  không được tự tạo Category mới
            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = book.Id }, book);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    // PUT: api/BookAPI/5
    // Content-Type: multipart/form-data
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromForm] Book book, IFormFile? imageFile)
    {
        try
        {
            var existing = await _context.Books.FindAsync(id);
            if (existing == null) return NotFound();

            // update fields
            existing.Title = book.Title;
            existing.Author = book.Author;
            existing.Price = book.Price;
            existing.CategoryId = book.CategoryId;

            if (imageFile != null && imageFile.Length > 0)
            {
                // delete old image if exists
                if (!string.IsNullOrEmpty(existing.ImageName))
                {
                    var oldPath = Path.Combine(_imageFolder, existing.ImageName);
                    if (System.IO.File.Exists(oldPath))
                        System.IO.File.Delete(oldPath);
                }

                var ext = Path.GetExtension(imageFile.FileName);
                var fileName = $"{Guid.NewGuid()}{ext}";
                var filePath = Path.Combine(_imageFolder, fileName);
                await using (var stream = System.IO.File.Create(filePath))
                {
                    await imageFile.CopyToAsync(stream);
                }
                existing.ImageName = fileName;
            }

            _context.Books.Update(existing);
            await _context.SaveChangesAsync();

            return Ok(existing);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    // DELETE: api/BookAPI/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var existing = await _context.Books.FindAsync(id);
            if (existing == null) return NotFound();

            // delete image file if exists
            if (!string.IsNullOrEmpty(existing.ImageName))
            {
                var oldPath = Path.Combine(_imageFolder, existing.ImageName);
                if (System.IO.File.Exists(oldPath))
                    System.IO.File.Delete(oldPath);
            }

            _context.Books.Remove(existing);
            await _context.SaveChangesAsync();

            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}
