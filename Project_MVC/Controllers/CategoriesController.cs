using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Project_MVC.Models;
using Project_MVC.Services;

namespace Project_MVC.Controllers
{
    public class CategoriesController : Controller
    {
        private ICRUDService<Category> mySQLProductCategoryService;

        public CategoriesController()
        {
            mySQLProductCategoryService = new MySQLCategoryService();
        }

        public ActionResult GetListLevelOneProductCategories()
        {
            //Console.WriteLine("123");
            //var list = db.ProductCategories.Where(s => s.Status != ProductCategoryStatus.Deleted).ToList();
            //var list = mySQLProductCategoryService.GetList().Where(s => Regex.IsMatch(s.Code, "^[A-Z]+$"));
            var list = mySQLProductCategoryService.GetList().Where(s => string.IsNullOrEmpty(s.ParentCode));
            var newlist = list.Select(dep => new
            {
                dep.Code,
                dep.Name
            });
            return new JsonResult()
            {
                Data = newlist,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
            };
        }

        // GET: ProductCategories
        public ActionResult Index()
        {
            return View(mySQLProductCategoryService.GetList());
        }

        // GET: ProductCategories/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category productCategory = mySQLProductCategoryService.Detail(id);
            if (productCategory == null)
            {
                return HttpNotFound();
            }
            return View(productCategory);
        }

        // GET: ProductCategories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProductCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Code,Name,Description,ParentCode")] Category productCategory, IEnumerable<HttpPostedFileBase> images)
        {
            if (mySQLProductCategoryService.CreateWithImage(productCategory, ModelState, images, null))
            {
                return RedirectToAction("Index");
            }

            return View(productCategory);
        }

        // GET: ProductCategories/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category productCategory = mySQLProductCategoryService.Detail(id);
            if (productCategory == null)
            {
                return HttpNotFound();
            }
            return View(productCategory);
        }

        // POST: ProductCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Code,Name,Description,CreatedAt,UpdatedAt,DeletedAt,CreatedBy,UpdatedBy,DeletedBy,Status,LevelOneProductCategoryCode")] Category productCategory)
        {
            //if (ModelState.IsValid)
            //{
            //    db.Entry(productCategory).State = EntityState.Modified;
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}
            return View(productCategory);
        }

        // GET: ProductCategories/Delete/5
        //public ActionResult Delete(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    ProductCategory productCategory = db.ProductCategories.Find(id);
        //    if (productCategory == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(productCategory);
        //}

        // POST: ProductCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            //ProductCategory productCategory = db.ProductCategories.Find(id);
            //db.ProductCategories.Remove(productCategory);
            //db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                mySQLProductCategoryService.DisposeDb();
            }
            base.Dispose(disposing);
        }
    }
}
