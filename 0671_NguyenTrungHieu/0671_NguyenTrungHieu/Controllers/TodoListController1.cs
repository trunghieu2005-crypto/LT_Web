using Microsoft.AspNetCore.Mvc;
using _0671_NguyenTrungHieu.Models;

namespace _0671_NguyenTrungHieu.Controllers
{
    public class TodoListController1 : Controller
    {

        private static List<TodoJob> _jobs = new List<TodoJob>()
        {
            new TodoJob{ MaCongViec = 1, TenCongViec = "Đi chợ", HoanThanh= true },
            new TodoJob{ MaCongViec = 2, TenCongViec = "Chơi thể thao", HoanThanh= false },
            new TodoJob{ MaCongViec = 3, TenCongViec = "Chơi game", HoanThanh= false },
        };
        public IActionResult Index()
        {
            return View(_jobs);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(TodoJob job)
        {
            job.MaCongViec = _jobs.Count + 1;
            _jobs.Add(job);
            return RedirectToAction("Index");
        }

        // 3. Sửa (Edit) - GET: Lấy dữ liệu cũ đổ vào form
        public IActionResult Edit(int id)
        {
            var job = _jobs.FirstOrDefault(j => j.MaCongViec == id);
            return View(job);
        }

        [HttpPost]
        public IActionResult Edit(TodoJob updatedJob)
        {
            var job = _jobs.FirstOrDefault(j => j.MaCongViec == updatedJob.MaCongViec);
            if (job != null)
            {
                job.TenCongViec = updatedJob.TenCongViec;
                job.HoanThanh = updatedJob.HoanThanh;
            }
            return RedirectToAction("Index");
        }

        // 4. Xóa (Delete)
        public IActionResult Delete(int id)
        {
            var job = _jobs.FirstOrDefault(j => j.MaCongViec == id);
            if (job != null) _jobs.Remove(job);
            return RedirectToAction("Index");
        }

        //Chi tiết danh sách công việc
        public ActionResult Details(int id)
        {
            var job = _jobs.FirstOrDefault(t => t.MaCongViec == id);
            return View(job);
        }


    }
}
