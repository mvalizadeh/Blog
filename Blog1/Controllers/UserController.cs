using Blog1.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blog1.Controllers
{
    public class UserController : Controller
    {
        BlogEntities db = new BlogEntities();
        public ActionResult Index()
        {
            return View(db.Users);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create([Bind(Include = "firstName,lastName,email,password,gender")] Users user, HttpPostedFileBase image)
        {
            string imageName = "user.jpg";
            if (ModelState.IsValid)
            {
                if (Request["password"] != Request["confirm-password"])
                {
                    ModelState.AddModelError("password", "پسوردها یکسان نیستند");
                    return View();
                }

                if (image != null)
                {
                    if (image.ContentType != "image/jpeg" && image.ContentType != "image/png")
                    {
                        ModelState.AddModelError("image", "فرمت فایل باید jpg یا png باشد.");
                        return View();
                    }

                    if (image.ContentLength > 500000)
                    {
                        ModelState.AddModelError("image", "حجم فایل نباید بزرگتر از 500 کیلوبایت باشد");
                        return View();
                    }

                    imageName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                    image.SaveAs(Server.MapPath("/images/profiles/" + imageName));
                }

                user.image = imageName;
                user.registerDate = DateTime.Now;
                user.is_active = true;
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            var user = db.Users.Find(id);

            if (user == null)
            {
                return HttpNotFound();
            }

            return View(user);
        }


        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            var user = db.Users.Find(id);

            if (user == null)
            {
                return HttpNotFound();
            }

            return View(user);
        }

        [HttpPost]
        public ActionResult Edit([Bind(Include = "id,firstName,lastName,email,password,image,registerDate,gender,is_active")] Users user, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    if (image.ContentType != "image/jpeg" && image.ContentType != "image/png")
                    {
                        ModelState.AddModelError("image", "فرمت فایل باید jpg یا png باشد.");
                        return View();
                    }

                    if (image.ContentLength > 500000)
                    {
                        ModelState.AddModelError("image", "حجم فایل نباید بزرگتر از 500 کیلوبایت باشد");
                        return View();
                    }

                    string imageName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                    image.SaveAs(Server.MapPath("/images/profiles/" + imageName));
                    user.image = imageName;
                }

                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }


        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            var user = db.Users.Find(id);

            if (user == null)
            {
                return HttpNotFound();
            }

            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteUser(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            var user = db.Users.Find(id);

            if (user == null)
            {
                return HttpNotFound();
            }

            if (user.image != "user.jpg")
            {
                System.IO.File.Delete(Server.MapPath("/images/profiles/" + user.image));
            }

            db.Users.Remove(user);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}