using Project_MVC.Models;
using Project_MVC.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Project_MVC.Controllers
{
    [Authorize(Roles = Constant.Admin + "," + Constant.Employee)]
    public class ProductImagesController : Controller
    {

        private IImageService imageService;
       
        public ProductImagesController()
        {
            imageService = new MySQLImageService();
        }


        public ActionResult Index(string searchString, string currentFilter, int? page)
        {
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var flowerImages = imageService.GetList().Where(s => !string.IsNullOrEmpty(s.FlowerCode));

            if (!String.IsNullOrEmpty(searchString))
            {
                flowerImages = flowerImages.Where(s => s.Flower.Name.Contains(searchString));
            }

            int pageSize = Constant.PageSize;
            int pageNumber = (page ?? 1);
            ThisPage thisPage = new ThisPage()
            {
                CurrentPage = pageNumber,
                TotalPage = Math.Ceiling((double)flowerImages.Count() / pageSize)
            };
            ViewBag.Page = thisPage;
            // nếu page == null thì lấy giá trị là 1, nếu không thì giá trị là page
            //return View(students.ToList().ToPagedList(pageNumber, pageSize));
            return View(flowerImages.OrderBy(s => s.FlowerCode).Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToList());
        }



        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FlowerImage productImage = imageService.DetailImage(id);
            if (productImage == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            imageService.DeleteImage(productImage);

            return RedirectToAction("Index");
        }

    }

}