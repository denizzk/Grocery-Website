using Grocery.Models.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Grocery.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        GroceryDBEntities db = new GroceryDBEntities();

        public ActionResult Index()
        {
            var model = db.Item.ToList();
            return View(model);
        }
    }
}