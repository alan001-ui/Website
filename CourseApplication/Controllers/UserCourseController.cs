using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CourseApplication.Models;
using System.IO;
using System.Globalization;

namespace CourseApplication.Controllers
{
    [Authorize(Roles = "User")]
    public class UserCourseController : Controller
    {

        public ActionResult Index()
        {
            return View();

        }

        public ActionResult GetData()
        {
            using (UserDBModel db = new UserDBModel())
            {
                List<UserCourse> courList = db.UserCourses.ToList<UserCourse>();
                return Json(new { data = courList }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpGet]
        public ActionResult AddOrEdit(int id = 0)
        {
            if (id == 0)
                return View(new UserCourse());
            else
            {
                using (UserDBModel db = new UserDBModel())
                {
                    return View(db.UserCourses.Where(x => x.CourseID == id).FirstOrDefault<UserCourse>());
                }
            }
        }

        [HttpPost]
        public ActionResult AddOrEdit(UserCourse cour)
        {
            using (UserDBModel db = new UserDBModel())
            {
                if (cour.CourseID == 0)
                {
                    db.UserCourses.Add(cour);
                    db.SaveChanges();
                    return Json(new { success = true, message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    db.Entry(cour).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json(new { success = true, message = "Book Successfully" }, JsonRequestBehavior.AllowGet);
                }
            }


        }

        public ActionResult Record()
        {
            return View();
        }
        public ActionResult GetList()
        {
            List<UserCourse> courList = new List<UserCourse>();
            using (UserDBModel db = new UserDBModel())
            {
                courList = db.UserCourses.ToList<UserCourse>();
                return Json(new { data = courList }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult Email()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Email(EmailFormModel model)
        {
            if (ModelState.IsValid)
            {
                var body = "<p>Email From: {0} ({1})</p><p>Message:</p><p>{2}</p>";
                var message = new MailMessage();
                message.To.Add(new MailAddress("nsy629800520@gmail.com"));  // replace with valid value 
                message.From = new MailAddress("nsy629800520@outlook.com");  // replace with valid value
                message.Subject = "Send File";
                message.Body = string.Format(body, model.FromName, model.FromEmail, model.Message);
                message.IsBodyHtml = true;
                if (model.Upload != null && model.Upload.ContentLength > 0)
                {
                    message.Attachments.Add(new Attachment(model.Upload.InputStream, Path.GetFileName(model.Upload.FileName)));
                }

                using (var smtp = new SmtpClient())
                {
                    var credential = new NetworkCredential
                    {
                        UserName = "nsy629800520@outlook.com",  // replace with valid value
                        Password = "Icoolen00"  // replace with valid value
                    };
                    smtp.Credentials = credential;
                    smtp.Host = "smtp-mail.outlook.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    await smtp.SendMailAsync(message);
                    return RedirectToAction("Sent");
                }
            }
            return View(model);
        }

        public ActionResult Sent()
        {
            return View();
        }

        public ActionResult BarChart()
        {
            UserDBModel entities = new UserDBModel();
            return Json(entities.UserCourses.ToList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Calendar() 
        {
            return View();
        }

        public JsonResult GetEvents()
        {
          using (CanlendarEntities db = new CanlendarEntities())
        {
          var events = db.Calendars.ToList();
          return new JsonResult { Data = events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        }


    }
}

        






