﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity.Owin;
using Project_MVC.Models;
using Project_MVC.Utils;
using static Project_MVC.Models.Flower;
using static Project_MVC.Models.Order;

namespace Project_MVC.Services
{
    public class MySQLOrderService : IOrderService
    {
        private MyDbContext _db;
        public MyDbContext DbContext
        {
            get { return _db ?? HttpContext.Current.GetOwinContext().Get<MyDbContext>(); }
            set { _db = value; }
        }

        private IUserService userService;
        private ICRUDService<Flower> mySQLFlowerService;

        public MySQLOrderService()
        {
            userService = new UserService();
            mySQLFlowerService = new MySQLFlowerService();
        }

        //public int? Create(Order item)
        //{
        //    var existOrder = DbContext.Orders.Where(s => s.ShipName == item.ShipName).ToList();
        //    var code = item.OrderDetails.FirstOrDefault().FlowerCode;
        //    var existOrderDetail = DbContext.OrderDetails.Where(s => s.FlowerCode == code).ToList();
        //    if (existOrder != null && existOrder.Count > 0 && existOrderDetail != null && existOrderDetail.Count > 0)
        //        return existOrderDetail.FirstOrDefault().OrderId;

        //    item.CreatedAt = DateTime.Now;
        //    item.UpdatedAt = null;
        //    item.DeletedAt = null;
        //    item.CreatedBy = userService.GetCurrentUserName();
        //    item.Status = OrderStatus.NotDeleted;
        //    DbContext.Orders.Add(item);
        //    DbContext.SaveChanges();

        //    return item.Id;
        //}

        //public bool CreateWithImage(Order item, ModelStateDictionary state, IEnumerable<HttpPostedFileBase> images, IEnumerable<HttpPostedFileBase> videos)
        //{
        //    throw new NotImplementedException();
        //}

        public bool Delete(Order item, ModelStateDictionary state)
        {
            if (state.IsValid)
            {
                item.Status = OrderStatus.Deleted;
                item.DeletedAt = DateTime.Now;
                item.DeletedBy = userService.GetCurrentUserName();
                DbContext.Orders.AddOrUpdate(item);
                DbContext.SaveChanges();

                return true;
            }

            return false;
        }

        public Order Detail(int? id)
        {
            return DbContext.Orders.Find(id);
        }

        public void DisposeDb()
        {
            DbContext.Dispose();
        }

        public IEnumerable<Order> GetList()
        {
            return DbContext.Orders.Where(s => s.Status != OrderStatus.Deleted);
        }

        public int? UpdateStatus(Order item, string userName)
        {
            item.UpdatedAt = DateTime.Now;
            item.UpdatedBy = userName;
            item.Status = OrderStatus.Paid;
            DbContext.Orders.AddOrUpdate(item);
            DbContext.SaveChanges();

            return item.Id;
        }

        public bool UpdateWithImage(Order existItem, Order item, ModelStateDictionary state, IEnumerable<HttpPostedFileBase> images)
        {
            throw new NotImplementedException();
        }
        public void Validate(Order item, ModelStateDictionary state)
        {
            throw new NotImplementedException();
        }

        public void ValidateCategory(Order item, ModelStateDictionary state)
        {
            throw new NotImplementedException();
        }

        public bool ValidateStringCode(string code)
        {
            throw new NotImplementedException();
        }

        public bool Update(Order existItem, Order item, ModelStateDictionary state)
        {
            if (state.IsValid)
            {
                existItem.ShipName = item.ShipName;
                existItem.ShipPhone = item.ShipPhone;
                existItem.ShipAddress = item.ShipAddress;
                existItem.PaymentTypeId = item.PaymentTypeId;
                existItem.Status = item.Status;
                existItem.UpdatedAt = DateTime.Now;
                existItem.UpdatedBy = userService.GetCurrentUserName();
                DbContext.Orders.AddOrUpdate(existItem);
                DbContext.SaveChanges();

                return true;
            }

            return false;
            //throw new NotImplementedException();
        }

        public IEnumerable<RevenueModel> GetListRevenues(string start, string end)
        {
            var orders = DbContext.Orders.Where(s => s.Status == OrderStatus.Done).ToList();
            var compareStartDate = DateTime.Now.AddDays(-29);
            var compareEndDate = DateTime.Now;

            if (!string.IsNullOrEmpty(start) && !string.IsNullOrEmpty(end))
            {
                compareStartDate = Utility.GetNullableDate(start).Value.Date + new TimeSpan(0, 0, 0);
                compareEndDate = Utility.GetNullableDate(end).Value.Date + new TimeSpan(23, 59, 59);
            }

            orders = orders.Where(s => s.UpdatedAt >= compareStartDate && s.UpdatedAt <= compareEndDate).ToList();
            var lstRevenues = new List<RevenueModel>();
            orders = orders.OrderBy(s => s.UpdatedAt).ToList();
            if (orders != null && orders.Count > 0)
            {
                foreach (var item in orders)
                {
                    lstRevenues.Add(new RevenueModel()
                    {
                        TimeGetRevenue = item.UpdatedAt.Value,
                        TotalRevenue = item.TotalPrice
                    });
                }
            }

            return lstRevenues;
        }

        public IEnumerable<RevenuePieChartModel> GetListRevenuesForPieChart(string start, string end)
        {
            var orders = DbContext.Orders.Where(s => s.Status == OrderStatus.Done);

            if (!string.IsNullOrEmpty(start))
            {
                var compareStartDate = Utility.GetNullableDate(start).Value.Date + new TimeSpan(0, 0, 0);
                orders = orders.Where(s => (s.UpdatedAt >= compareStartDate));
            }
            else
            {
                var compareStartDate = DateTime.Now.AddDays(-29);
                orders = orders.Where(s => (s.UpdatedAt >= compareStartDate));
            }
            if (!string.IsNullOrEmpty(end))
            {
                var compareEndDate = Utility.GetNullableDate(end).Value.Date + new TimeSpan(23, 59, 59);
                orders = orders.Where(s => (s.UpdatedAt <= compareEndDate));
            }
            else
            {
                var compareEndDate = DateTime.Now;
                orders = orders.Where(s => (s.UpdatedAt <= compareEndDate));
            }

            var lstRevenues = new List<RevenuePieChartModel>();
            orders = orders.OrderBy(s => s.UpdatedAt);
            if (orders != null && orders.ToList().Count > 0)
            {
                lstRevenues = orders.SelectMany(s => s.OrderDetails).GroupBy(s => s.FlowerCode).Select(cl => new RevenuePieChartModel
                {
                    FlowerName = cl.FirstOrDefault().Flower.Name,
                    TotalRevenue = cl.Sum(c => (c.UnitPrice * c.Quantity))
                }).ToList();
            }

            return lstRevenues;
        }
    }

    //public IEnumerable<RevenueModel> GetListRevenuesYear()
    //{
    //    var thisYear = DateTime.Now.Year;
    //    var lstOrder = DbContext.Orders.Where(s => s.Status == OrderStatus.Paid || s.Status == OrderStatus.Done && s.UpdatedAt.Value.Year >= thisYear - 4).ToList();
    //    var lstRevenuesYear = new List<RevenueModel>();
    //    if (lstOrder != null && lstOrder.ToList().Count > 0)
    //    {
    //        for (var i = thisYear - 4; i <= thisYear; i++)
    //        {
    //            lstRevenuesYear.Add(new RevenueModel()
    //            {
    //                RevenueOf = string.Format("{0}", i),
    //                TotalRevenue = lstOrder.Where(s => s.UpdatedAt.Value.Year == i).Sum(s => s.TotalPrice)
    //            });
    //        }
    //    }

    //    return lstRevenuesYear;
    //}
}
