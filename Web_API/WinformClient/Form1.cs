using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using Newtonsoft.Json;
using System.IO;

namespace WinformClient
{
    public partial class Form1 : Form
    {
        private readonly HttpClient _http = new HttpClient();
        public Form1()
        {
            InitializeComponent();
        }

        private class CategoryItem
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public CategoryItem(int id, string name) { Id = id; Name = name; }
            public override string ToString() => Name;
        }

        private class BookDto
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Author { get; set; }
            public decimal Price { get; set; }
            public string ImageName { get; set; }
            public string Description { get; set; }
            public CategoryDto Category { get; set; }
        }

        private class CategoryDto
        {
            public int Id { get; set; }
            public string Name { get; set; }

            public override string ToString()
            {
                return Name;
            }
        }

        private async void Form1_Load_1(object sender, EventArgs e)
        {
            btnBrowseImage.Click += btnBrowseImage_Click;
            btnSearch.Click += btnSearch_Click;
            btnAdd.Click += btnAdd_Click;

            cboCategory.Items.Clear();
            cboCategory.Items.Add(new CategoryItem(2, "Công nghệ"));
            cboCategory.SelectedIndex = 0;

            await LoadBooksAsync(); // Gọi trực tiếp như thế này là chuẩn nhất
        }

        private async Task LoadBooksAsync()
        {
            try
            {
                var search = string.Empty;
                if (InvokeRequired)
                {
                    search = (string)Invoke(new Func<string>(() => txtSearch.Text.Trim()));
                }
                else
                {
                    search = txtSearch.Text.Trim();
                }

                var url = string.IsNullOrEmpty(search) ? "http://localhost:9999/api/BookAPI" : $"http://localhost:9999/api/BookAPI?search={Uri.EscapeDataString(search)}";
                var resp = await _http.GetAsync(url);
                resp.EnsureSuccessStatusCode();
                var json = await resp.Content.ReadAsStringAsync();
                var books = JsonConvert.DeserializeObject<List<BookDto>>(json) ?? new List<BookDto>();

                // bind to grid (invoke on UI thread)
                if (dgvBooks.InvokeRequired)
                {
                    dgvBooks.Invoke(new Action(() => dgvBooks.DataSource = books));
                }
                else
                {
                    dgvBooks.DataSource = books;
                }

                // populate categories
                var categories = books.Where(b => b.Category != null).Select(b => b.Category).GroupBy(c => c.Id).Select(g => g.First()).ToList();
                if (cboCategory.InvokeRequired)
                {
                    cboCategory.Invoke(new Action(() =>
                    {
                        cboCategory.Items.Clear();
                        foreach (var c in categories) cboCategory.Items.Add(new CategoryItem(c.Id, c.Name));
                        if (cboCategory.Items.Count > 0) cboCategory.SelectedIndex = 0;
                    }));
                }
                else
                {
                    cboCategory.Items.Clear();
                    foreach (var c in categories) cboCategory.Items.Add(new CategoryItem(c.Id, c.Name));
                    if (cboCategory.Items.Count > 0) cboCategory.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("LoadBooks error: " + ex.Message);
            }
        }

        private void btnBrowseImage_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtImagePath.Text = openFileDialog1.FileName;
            }
        }

        private async void btnSearch_Click(object sender, EventArgs e)
        {
            await LoadBooksAsync();
        }

        private async void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                using (var content = new MultipartFormDataContent())
                {
                    content.Add(new StringContent(txtTitle.Text ?? ""), "Title");
                    content.Add(new StringContent(txtAuthor.Text ?? ""), "Author");
                    content.Add(new StringContent("0"), "Price"); // Nếu chưa có txtPrice thì để 0
                    content.Add(new StringContent(txtDescription.Text ?? ""), "Description");

                    // KIỂM TRA KỸ CHỖ NÀY
                    if (cboCategory.SelectedItem is CategoryItem ci)
                    {
                        content.Add(new StringContent(ci.Id.ToString()), "CategoryId");
                    }
                    else
                    {
                        MessageBox.Show("Vui lòng chọn loại sách!");
                        return;
                    }

                    var path = txtImagePath.Text;
                    if (!string.IsNullOrEmpty(path) && File.Exists(path))
                    {
                        byte[] fileBytes = File.ReadAllBytes(path);
                        var fileContent = new ByteArrayContent(fileBytes);
                        fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg");
                        content.Add(fileContent, "imageFile", Path.GetFileName(path));
                    }

                    var resp = await _http.PostAsync("http://localhost:9999/api/BookAPI", content);
                    if (resp.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Thêm thành công!");
                        await LoadBooksAsync();
                    }
                    else
                    {
                        var error = await resp.Content.ReadAsStringAsync();
                        MessageBox.Show("Lỗi từ API: " + error); // Nó sẽ hiện rõ thiếu trường nào
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("Lỗi hệ thống: " + ex.Message); }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

       
    }
}
