using Microsoft.EntityFrameworkCore;
using Web_API.Data;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// 1. Chạy trên port 9999 (Dùng HTTP thường để tránh lỗi chứng chỉ)
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(9999);
});

builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); // Thêm cái này để có giao diện Swagger

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", p => p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

builder.Services.AddDbContext<BookDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// 2. Bật Swagger UI để Hiếu dễ test
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 3. Xử lý thư mục ảnh
var imageFolder = Path.Combine(Directory.GetCurrentDirectory(), "Content", "ImageBooks");
if (!Directory.Exists(imageFolder))
    Directory.CreateDirectory(imageFolder);

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(imageFolder),
    RequestPath = "/Content/ImageBooks"
});

app.UseCors("AllowAll");
// app.UseHttpsRedirection(); // Tạm tắt cái này đi để chạy port 9999 mượt hơn
app.UseAuthorization();
app.MapControllers();

app.Run();