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
        private ICRUDService<Product> mySQLProductService;
        private ICRUDService<OwnerOfCourse> mySQLOwnerOfCourseService;

        public HomeController()
        {
            mySQLProductService = new MySQLProductService();
            mySQLOwnerOfCourseService = new MySQLOwnerOfCourseService();
        }

        public ActionResult Index()
        {
            var list = mySQLProductService.GetList();
            return View(list);
        }

        public ActionResult About()
        {
            return View(mySQLOwnerOfCourseService.GetList());
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