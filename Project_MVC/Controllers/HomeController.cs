using Project_MVC.Models;
using Project_MVC.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project_MVC.Controllers
{
    public class HomeController : Controller
    {
        private ICRUDService<Category> mySQLCategoryService;

        public HomeController()
        {
            mySQLCategoryService = new MySQLCategoryService();
        }

        public ActionResult Index()
        {
            var list = mySQLCategoryService.GetList();
            return View(list);
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Blog()
        {
            return View();
        }

        public ActionResult Blog_Single()
        {
            return View();
        }
    }
}