using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CourseApplication.Models;


namespace CourseApplication.Controllers
{
    [Authorize(Roles =("Administrator"))]
    
    public class CourseController : Controller
    {
      
        public ActionResult Index()
        {
           return View();
            
        }

        public ActionResult GetData()
        {
            using (DBModel db = new DBModel())
            {
                List<Course> courList = db.Courses.ToList<Course>();
                return Json(new { data = courList }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpGet]
        public ActionResult AddOrEdit(int id = 0)
        {
            if (id == 0)
                return View(new Course());
            else
            {
                using (DBModel db = new DBModel())
                {
                    return View(db.Courses.Where(x => x.CourseID == id).FirstOrDefault<Course>());
                }
            }
        }

        [HttpPost]
        public ActionResult AddOrEdit(Course cour)
        {
            using (DBModel db = new DBModel())
            {
                if (cour.CourseID == 0)
                {
                    db.Courses.Add(cour);
                    db.SaveChanges();
                    return Json(new { success = true, message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    db.Entry(cour).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json(new { success = true, message = "Updated Successfully" }, JsonRequestBehavior.AllowGet);
                }
            }


        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            using (DBModel db = new DBModel())
            {
                Course cour = db.Courses.Where(x => x.CourseID == id).FirstOrDefault<Course>();
                db.Courses.Remove(cour);
                db.SaveChanges();
                return Json(new { success = true, message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
            }
        }
    }

}

