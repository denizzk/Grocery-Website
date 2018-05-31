using Grocery.Models.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Grocery.Controllers
{
    [AllowAnonymous]
    public class SecurityController : Controller
    {
        GroceryDBEntities db = new GroceryDBEntities();

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(User account)
        {
            if (ModelState.IsValid)
            {
                var usr = db.User.Where(m => m.Username == account.Username || m.Email == account.Email).FirstOrDefault();
                if (usr != null)
                {
                    ModelState.AddModelError("", "Username or Email is already exist.");
                    return View();
                }

                db.User.Add(account);
                db.SaveChanges();
                TempData["succesRegistered"] = account.Username + " succesfully registered.";
                return RedirectToAction("Login");
            }
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(User user)
        {
            var usr = db.User.FirstOrDefault(u => u.Username == user.Username && u.Password == user.Password);
            if (usr != null)
            {
                Session["UserID"] = usr.Id.ToString();
                Session["Username"] = usr.Username.ToString();
                FormsAuthentication.SetAuthCookie(usr.Username, true);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "Username or Password is wrong.");
            }
            return View();
        }
        
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}