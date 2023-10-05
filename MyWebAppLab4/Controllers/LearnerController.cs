using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyWebAppLab4.Data;
using MyWebAppLab4.Models;
using System.Security.Cryptography.X509Certificates;

namespace MyWebAppLab4.Controllers
{
    public class LearnerController : Controller
    {
        private readonly SchoolContext db;
        public LearnerController(SchoolContext context)
        {
            db = context;
        }
        public IActionResult Index()
        {
            var learners = db.Learners.Include(m => m.Major).ToList();
            return View(learners);
        }

        //thêm 2 action create
        public IActionResult Create()
        {
            //dùng 1 trong 2 cách để tạo SelectList gửi về View qua ViewBag để
            //hiển thị danh sách chuyên ngành (Majors)
            var majors = new List<SelectListItem>(); //cách 1
            foreach (var item in db.Majors)
            {
                majors.Add(new SelectListItem
                {
                    Text = item.MajorName,

                    Value = item.MajorID.ToString()
                });

            }
            ViewBag.MajorID = majors;
            ViewBag.MajorID = new SelectList(db.Majors, "MajorID", "MajorName"); //cách 2
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("FirstMidName,LastName,MajorID,EnrollmentDate")] Learner learner)
        {
            if (ModelState.IsValid)
            {
                db.Learners.Add(learner);
                db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            //lại dùng 1 trong 2 cách tạo SelectList gửi về View để hiển thị danh sách Majors
            ViewBag.MajorID = new SelectList(db.Majors, "MajorID", "MajorName");
            return View();
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var obj = db.Learners.Find(id);
            if (obj == null)
            {
                return NotFound();
            }

            db.Learners.Remove(obj);
            db.SaveChanges(true);

            return RedirectToAction("Index");
        }


        [HttpGet]

        public IActionResult Edit(int? id)
        {
            var obj = db.Learners.Find(id);
            if (obj == null)
            {
                return NotFound();
            }

          

            return RedirectToAction("Index");
        }

    }
}
