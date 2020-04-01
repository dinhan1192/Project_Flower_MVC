using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
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
    public class DashBoardController : Controller
    {
        private IOrderService mySQLOrderService;

        public DashBoardController()
        {
            mySQLOrderService = new MySQLOrderService();
        }

        // GET: DashBoard
        public ActionResult Index(string start, string end)
        {
            var compareDate = new DateTimeModel();
            var orders = mySQLOrderService.GetList();

            if (!string.IsNullOrEmpty(start))
            {
                var compareStartDate = Utility.GetNullableDate(start).Value.Date + new TimeSpan(0, 0, 0);
                orders = orders.Where(s => (s.UpdatedAt >= compareStartDate));
                compareDate.startDate = compareStartDate;
            }
            if (!string.IsNullOrEmpty(end))
            {
                var compareEndDate = Utility.GetNullableDate(end).Value.Date + new TimeSpan(23, 59, 59);
                orders = orders.Where(s => (s.UpdatedAt <= compareEndDate));
                compareDate.endDate = compareEndDate;
            }

            ViewBag.CompareDate = compareDate;

            var lstRevenues = mySQLOrderService.GetListRevenues(start, end);
            var dataPoints = new List<DataPoint>();
            lstRevenues.ForEach(s =>
            {
                if (s.MonthYear == null)
                {
                    dataPoints.Add(new DataPoint(s.TimeGetRevenue, s.TotalRevenue));
                }
                else
                {
                    dataPoints.Add(new DataPoint(s.TotalRevenue, s.MonthYear));
                }
            });
            ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);
            return View();
        }

        //public ActionResult IndexYear()
        //{
        //    var lstRevenueYear = orderService.GetListRevenuesYear();
        //    var dataPoints = new List<DataPoint>();
        //    foreach (var item in lstRevenueYear)
        //    {
        //        dataPoints.Add(new DataPoint(item.RevenueOf, item.TotalRevenue));
        //    }

        //    ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);
        //    return View();
        //}
    }
}