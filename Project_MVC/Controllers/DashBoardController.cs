using Newtonsoft.Json;
using Project_MVC.Models;
using Project_MVC.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project_MVC.Controllers
{
    public class DashBoardController : Controller
    {
        private IOrderService orderService;

        public DashBoardController()
        {
            orderService = new MySQLOrderService();
        }

        // GET: DashBoard
        public ActionResult IndexMonth(string searchString, string currentFilter)
        {
            if (string.IsNullOrEmpty(searchString))
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;
            var lstRevenueMonth = orderService.GetListRevenuesMonth(searchString);
            var dataPoints = new List<DataPoint>();
            foreach (var item in lstRevenueMonth)
            {
                dataPoints.Add(new DataPoint(item.RevenueOf, item.TotalRevenue));
            }
            //List<DataPoint> dataPoints = new List<DataPoint>{
            //    new DataPoint("1", 100),
            //    new DataPoint("2", 236),
            //    new DataPoint("3", 142),
            //    new DataPoint("4", 351),
            //    new DataPoint("5", 646),
            //    new DataPoint("6", 746),
            //    new DataPoint("7", 246),
            //    new DataPoint("8", 846),
            //    new DataPoint("9", 946),
            //    new DataPoint("10", 2246),
            //    new DataPoint("11", 2346),
            //    new DataPoint("12", 1546),
            //};

            ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);
            return View();
        }

        public ActionResult IndexYear()
        {
            var lstRevenueYear = orderService.GetListRevenuesYear();
            var dataPoints = new List<DataPoint>();
            foreach (var item in lstRevenueYear)
            {
                dataPoints.Add(new DataPoint(item.RevenueOf, item.TotalRevenue));
            }

            ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);
            return View();
        }
    }
}