using Newtonsoft.Json;
using Project_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project_MVC.Controllers
{
    public class DashBoardController : Controller
    {
        // GET: DashBoard
        public ActionResult IndexMonth()
        {
            List<DataPoint> dataPoints = new List<DataPoint>{
                new DataPoint(1, 22),
                new DataPoint(2, 36),
                new DataPoint(3, 42),
                new DataPoint(4, 51),
                new DataPoint(5, 46),
                new DataPoint(6, 46),
                new DataPoint(7, 46),
                new DataPoint(8, 46),
                new DataPoint(9, 46),
                new DataPoint(10, 46),
                new DataPoint(11, 46),
                new DataPoint(12, 46),
            };

            ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);
            return View();
        }

        public ActionResult IndexYear()
        {
            List<DataPoint> dataPoints = new List<DataPoint>{
                new DataPoint(2020, 22),
                new DataPoint(2020, 36),
                new DataPoint(2020, 42),
                new DataPoint(2020, 51),
                new DataPoint(2020, 46),
            };

            ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);
            return View();
        }
    }
}