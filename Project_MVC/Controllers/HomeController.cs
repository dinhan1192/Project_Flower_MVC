using Project_MVC.Models;
using Project_MVC.Services;
using Project_MVC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project_MVC.Controllers
{
    public class HomeController : Controller
    {
        private ICRUDService<Flower> mySQLFlowerService;

        public HomeController()
        {
            mySQLFlowerService = new MySQLFlowerService();
        }

        public ActionResult Index()
        {
            var list = mySQLFlowerService.GetList();
            //SeedUtility.SeedRandomOrder(Constant.DeleteUnknownOrders);
            //SeedUtility.SeedRandomOrder(Constant.SeedRandomOrders);

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