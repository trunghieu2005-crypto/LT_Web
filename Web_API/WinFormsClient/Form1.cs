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
using System.Net.Http.Headers;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using System.Text;

namespace WinFormsClient
{
    public partial class Form1 : Form
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private string _selectedImagePath = null;

        public Form1()
        {
            InitializeComponent();
            _httpClient.BaseAddress = new Uri("http://localhost:9999/");
            this.Load += new EventHandler(Form1_Load);
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            await LoadCategoriesAsync();
            await SearchAsync();
        }

        private async Task LoadCategoriesAsync()
        {
            try
            {
                var resp = await _httpClient.GetAsync("api/BookAPI");
                resp.EnsureSuccessStatusCode();
                var json = await resp.Content.ReadAsStringAsync();
                var books = DeserializeJson<List<BookDto>>(json) ?? new List<BookDto>();
                var categories = books.Select(b => b.Category).Where(c => c != null).GroupBy(c => c.Id).Select(g => g.First()).ToList();
                cmbCategory.Items.Clear();
                foreach (var c in categories)
                {
                    cmbCategory.Items.Add(new ComboItem(c.Id, c.Name));
                }
                if (cmbCategory.Items.Count > 0) cmbCategory.SelectedIndex = 0;
            }
            catch
            {
                // ignore
            }
        }

        private async Task SearchAsync()
        {
            try
            {
                var q = txtTitle.Text?.Trim();
                var url = string.IsNullOrEmpty(q) ? "api/BookAPI" : $"api/BookAPI?search={Uri.EscapeDataString(q)}";
                var resp = await _httpClient.GetAsync(url);
                resp.EnsureSuccessStatusCode();
                var json = await resp.Content.ReadAsStringAsync();
                var books = DeserializeJson<List<BookDto>>(json) ?? new List<BookDto>();
                dgvBooks.DataSource = books;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private async void btnSearch_Click(object sender, EventArgs e)
        {
            await SearchAsync();
        }

        private void btnSelectImage_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    _selectedImagePath = ofd.FileName;
                    pbImage.Image = Image.FromFile(_selectedImagePath);
                }
            }
        }

        private async void btnAdd_Click(object sender, EventArgs e)
        {
            await AddBookAsync();
        }

        private async Task AddBookAsync()
        {
            try
            {
                using var content = new MultipartFormDataContent();
                // text fields
                content.Add(new StringContent(txtTitle.Text ?? string.Empty), "Title");
                content.Add(new StringContent(txtAuthor.Text ?? string.Empty), "Author");
                content.Add(new StringContent(txtPrice.Text ?? string.Empty), "Price");
                if (cmbCategory.SelectedItem is ComboItem ci)
                {
                    content.Add(new StringContent(ci.Id.ToString()), "CategoryId");
                }

                // file
                if (!string.IsNullOrEmpty(_selectedImagePath) && File.Exists(_selectedImagePath))
                {
                    var fs = File.OpenRead(_selectedImagePath);
                    var fileContent = new StreamContent(fs);
                    fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                    content.Add(fileContent, "imageFile", Path.GetFileName(_selectedImagePath));
                }

                var resp = await _httpClient.PostAsync("api/BookAPI", content);
                if (resp.IsSuccessStatusCode)
                {
                    MessageBox.Show("Added");
                    await SearchAsync();
                }
                else
                {
                    var err = await resp.Content.ReadAsStringAsync();
                    MessageBox.Show($"Error: {resp.StatusCode} - {err}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private class ComboItem
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public ComboItem(int id, string name) { Id = id; Name = name; }
            public override string ToString() { return Name; }
        }

        [DataContract]
        private class BookDto
        {
            [DataMember] public int Id { get; set; }
            [DataMember] public string Title { get; set; }
            [DataMember] public string Author { get; set; }
            [DataMember] public decimal Price { get; set; }
            [DataMember] public string ImageName { get; set; }
            [DataMember] public CategoryDto Category { get; set; }
        }

        [DataContract]
        private class CategoryDto
        {
            [DataMember] public int Id { get; set; }
            [DataMember] public string Name { get; set; }
        }

        private static T DeserializeJson<T>(string json) where T : class
        {
            var serializer = new DataContractJsonSerializer(typeof(T));
            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                return serializer.ReadObject(ms) as T;
            }
        }
    }
}

