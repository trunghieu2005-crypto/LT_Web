using Bt3_asp.Models;
using Microsoft.AspNetCore.Mvc;

namespace Bt3_asp.Controllers
{
    public class StudentController1 : Controller
    {
        private static List<Student> RegisteredStudents = new List<Student>();

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ShowKQ(Student student)
        {
            RegisteredStudents.Add(student);

            int sameMajorCount = RegisteredStudents.FindAll(s => s.ChuyenNganh == student.ChuyenNganh).Count;

            ViewBag.MSSV = student.MSSV;
            ViewBag.HoTen = student.HoTen;
            ViewBag.ChuyenNganh = student.ChuyenNganh;
            ViewBag.SameMajorCount = sameMajorCount;

            return View();
        }
    }
}
