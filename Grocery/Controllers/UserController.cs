using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Grocery.Models.EntityFramework;

namespace Grocery.Controllers
{
    [Authorize(Roles = "A")]
    public class UserController : Controller
    {
        private GroceryDBEntities db = new GroceryDBEntities();

        // GET: User
        public ActionResult Index()
        {
            return View(db.User.ToList());
        }

        // GET: User/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.User.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: User/Create
        public ActionResult Create()
        {
            ViewBag.Role = new SelectList(db.User.Distinct().ToList(), "Id", "Role");
            return View();
        }

        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(User user)
        {
            var IsUNExist = db.User.Any(m => m.Username == user.Username);
            var IsEMExist = db.User.Any(m => m.Email == user.Email);
            
            if (ModelState.IsValid && !IsUNExist && !IsEMExist)
            {
                db.User.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            if (IsUNExist) ModelState.AddModelError("", "Username is already exist.");
            else if (IsEMExist) ModelState.AddModelError("", "Email is already exist.");
            else ModelState.AddModelError("", "Fill the blanks correctly.");
            return View(user);
        }

        // GET: User/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.User.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(User user)
        {
            bool IsUNExist=false;
            var userExist = db.User.FirstOrDefault(m => m.Username == user.Username);
            if(userExist!=null)
                IsUNExist = (userExist.Id == user.Id) ? false : true;

            bool IsEMExist=false;
            userExist = db.User.FirstOrDefault(m => m.Email == user.Email);
            if (userExist != null)
                IsEMExist = (userExist.Id == user.Id) ? false : true;

            userExist = db.User.FirstOrDefault(m => m.Id == user.Id);

            if (ModelState.IsValid && !IsUNExist && !IsEMExist)
            {
                db.Entry(userExist).CurrentValues.SetValues(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            if (IsUNExist) ModelState.AddModelError("", "Username is already exist.");
            else if (IsEMExist) ModelState.AddModelError("", "Email is already exist.");
            else ModelState.AddModelError("", "Fill the blanks correctly.");
            return View(user);
        }

        // GET: User/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.User.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.User.Find(id);
            db.User.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
