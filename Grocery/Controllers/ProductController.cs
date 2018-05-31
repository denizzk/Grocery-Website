using Grocery.Models.EntityFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Grocery.Controllers
{
    [Authorize(Roles = "A")]
    public class ProductController : Controller
    {
        GroceryDBEntities db = new GroceryDBEntities();

        public ActionResult Index()
        {
            var model = db.Item.ToList();
            return View(model);
        }

        [HttpGet]
        public ActionResult New()
        {
            return View("ProductForm", new Item());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Item item, HttpPostedFileBase imageUrl)
        {
            if (!ModelState.IsValid)
            {
                return View("ProductForm", item);
            }            
            
            if (item.Id == 0)
            {
                if (imageUrl != null)
                {
                    string ImageName = System.IO.Path.GetFileName(imageUrl.FileName);
                    string physicalPath = Server.MapPath("~/UploadedImages/" + ImageName);

                    // save image in folder
                    imageUrl.SaveAs(physicalPath);

                    //save new record in database
                    item.ImageUrl = ImageName;
                    db.Item.Add(item);
                }
            }
            else
            {
                var updateItem = db.Item.Find(item.Id);
                if (updateItem == null)
                {
                    return HttpNotFound();
                }
                if (imageUrl != null)
                {
                    string ImageName = System.IO.Path.GetFileName(imageUrl.FileName);
                    string physicalPath = Server.MapPath("~/UploadedImages/" + ImageName);

                    // save image in folder
                    imageUrl.SaveAs(physicalPath);

                    //save new record in database
                    item.ImageUrl = ImageName;
                    db.Entry(updateItem).CurrentValues.SetValues(item);
                }
            }
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult Update(int id)
        {
            var model = db.Item.Find(id);
            if (model == null)
                return HttpNotFound();
            return View("ProductForm", model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var deleteItem = db.Item.Find(id);
            if (deleteItem == null)
                return HttpNotFound();
            db.Item.Remove(deleteItem);
            db.SaveChanges();
            return RedirectToAction("Index", "Product");
        }
    }
}