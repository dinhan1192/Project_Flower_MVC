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
    ////[Authorize(Roles = Constant.Admin + "," + Constant.Employee)]
    public class ProductImagesController : Controller
    {

        private IImageService imageService;

        public ProductImagesController()
        {
            imageService = new MySQLImageService();
        }
        public ActionResult Index() {
            return View(imageService.GetList());
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