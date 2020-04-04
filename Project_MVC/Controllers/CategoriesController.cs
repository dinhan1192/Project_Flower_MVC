using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Project_MVC.Models;
using Project_MVC.Services;

namespace Project_MVC.Controllers
{
    [Authorize(Roles = Constant.Admin + "," + Constant.Employee)]
    public class CategoriesController : Controller
    {
        private ICRUDService<Category> mySQLCategoryService;

        public CategoriesController()
        {
            mySQLCategoryService = new MySQLCategoryService();
        }

        public ActionResult Index(string sortOrder, string searchString, string currentFilter, int? page, string filter)
        {
            if (!string.IsNullOrEmpty(filter))
            {
                var filterThisPage = JsonConvert.DeserializeObject<ThisPage>(filter);
                currentFilter = filterThisPage.SearchString;
            }

            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            // lúc đầu vừa vào thì sortOrder là null, cho nên gán NameSortParm = name_desc
            // Ấn vào link Full name thì lúc đó NameSortParm có giá trị là name_desc, sortOrder trên view được gán = NameSortParm cho nên sortOrder != null
            // và NameSortParm = ""
            // Ấn tiếp vào link Full Name thì sortOrder = "" cho nên NameSortParm = name_desc
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var flowerCategories = mySQLCategoryService.GetList();

            if (!String.IsNullOrEmpty(searchString))
            {
                flowerCategories = flowerCategories.Where(s => s.Name.Contains(searchString) || s.Code.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    flowerCategories = flowerCategories.OrderByDescending(s => s.Name);
                    break;
                case "Date":
                    flowerCategories = flowerCategories.OrderBy(s => s.UpdatedAt);
                    break;
                case "date_desc":
                    flowerCategories = flowerCategories.OrderByDescending(s => s.UpdatedAt);
                    break;
                default:
                    flowerCategories = flowerCategories.OrderBy(s => s.Name);
                    break;
            }

            int pageSize = Constant.PageSize;
            int pageNumber = (page ?? 1);
            ThisPage thisPage = new ThisPage()
            {
                CurrentPage = pageNumber,
                TotalPage = Math.Ceiling((double)flowerCategories.Count() / pageSize),
                SearchString = searchString
            };
            ViewBag.Page = thisPage;
            // nếu page == null thì lấy giá trị là 1, nếu không thì giá trị là page
            //return View(students.ToList().ToPagedList(pageNumber, pageSize));
            return View(flowerCategories.Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToList());
        }
        public ActionResult GetListLevelOneProductCategories()
        {
            //Console.WriteLine("123");
            //var list = db.ProductCategories.Where(s => s.Status != ProductCategoryStatus.Deleted).ToList();
            //var list = mySQLProductCategoryService.GetList().Where(s => Regex.IsMatch(s.Code, "^[A-Z]+$"));
            var list = mySQLCategoryService.GetList().Where(s => string.IsNullOrEmpty(s.ParentCode));
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

       

        // GET: ProductCategories/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category flowerCategory = mySQLCategoryService.Detail(id);
            if (flowerCategory == null || flowerCategory.IsDeleted())
            {
                return HttpNotFound();
            }
            return View(flowerCategory);
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
        public ActionResult Create([Bind(Include = "Code,Name,Description,ParentCode")] Category category, string strImageUrl)
        {
            if (mySQLCategoryService.CreateWithImage(category, ModelState, strImageUrl, null))
            {
                return RedirectToAction("Index");
            }

            return View(category);
        }

        // GET: ProductCategories/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var flowerCategory = mySQLCategoryService.Detail(id);
            var levelOneCategory = mySQLCategoryService.Detail(flowerCategory.ParentNameAndCode);

            if (levelOneCategory == null)
            {
                flowerCategory.ParentNameAndCode = "";
            }
            else
            {
                flowerCategory.ParentNameAndCode = levelOneCategory.Code + " - " + levelOneCategory.Name;
            }
            if (flowerCategory == null || flowerCategory.IsDeleted())
            {
                return HttpNotFound();
            }
            return View(flowerCategory);
        }

        // POST: ProductCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Code,Name,Description,ParentCode")] Category flowerCategory, string strImageUrl)
        {
            if (flowerCategory == null || flowerCategory.Code == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var existFlowerCategory = mySQLCategoryService.Detail(flowerCategory.Code);
            if (existFlowerCategory == null || existFlowerCategory.IsDeleted())
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            if (mySQLCategoryService.UpdateWithImage(existFlowerCategory, flowerCategory, ModelState, strImageUrl))
            {
                return RedirectToAction("Index");
            }

            return View(flowerCategory);
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
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var existCategory = mySQLCategoryService.Detail(id);
            if (existCategory == null || existCategory.IsDeleted())
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            if (mySQLCategoryService.Delete(existCategory, ModelState))
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                mySQLCategoryService.DisposeDb();
            }
            base.Dispose(disposing);
        }
    }
}
