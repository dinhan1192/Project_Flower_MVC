using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Diagnostics;
using System.Globalization;
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
            var compareStartDate = DateTime.Now.AddDays(-29);
            var compareEndDate = DateTime.Now;

            if (!string.IsNullOrEmpty(start) && !string.IsNullOrEmpty(end))
            {
                compareStartDate = Utility.GetNullableDate(start).Value.Date + new TimeSpan(0, 0, 0);
                compareEndDate = Utility.GetNullableDate(end).Value.Date + new TimeSpan(23, 59, 59);
            }

            var orders = DbContext.Orders
                .Where(s => s.Status == OrderStatus.Done)
                .Where(s => s.CreatedAt >= compareStartDate && s.CreatedAt <= compareEndDate);
            TimeSpan diff = compareEndDate.Subtract(compareStartDate);
            var lstRevenues = new List<RevenueModel>();

            if (diff.TotalDays <= 7)
            {
                orders = orders.OrderBy(s => s.CreatedAt);
                var lstOrder = orders.ToList();
                if (orders != null && orders.ToList().Count > 0)
                {
                    orders.ForEach(s => lstRevenues.Add(
                        new RevenueModel()
                        {
                            TimeGetRevenue = s.CreatedAt.Value,
                            TotalRevenue = s.TotalPrice
                        })
                );
                }
            }
            else if(diff.TotalDays <= 31)
            {
                var lstParams = orders.GroupBy(s => DbFunctions.TruncateTime(s.CreatedAt))
                .Select(s => new { time = s.Key, revenue = s.Sum(o => o.TotalPrice) }).ToList();
                if (lstParams != null && lstParams.ToList().Count > 0)
                {
                    lstParams.ForEach(s => lstRevenues.Add(
                        new RevenueModel()
                        {
                            TimeGetRevenue = s.time ?? DateTime.Now,
                            TotalRevenue = s.revenue
                        })
                );
                }
            } else if (diff.TotalDays <= 365)
            {
                var lstParams = orders.OrderBy(s => s.CreatedAt).ToList();
                    
                var newlistParams = lstParams.GroupBy(s => s.CreatedAt.Value.Month.ToString().FirstOrDefault())
                .Select(s => new { time = s.Key, revenue = s.Sum(o => o.TotalPrice) }).ToList();
                if (newlistParams != null && newlistParams.ToList().Count > 0)
                {
                    newlistParams.ForEach(s => lstRevenues.Add(
                        new RevenueModel()
                        {
                            MonthYear = new DateTime(2018, Convert.ToInt32(s.time.ToString()), 1).ToString("MMMM yyyy", FlowerUtility.UnitedStates),
                            TotalRevenue = s.revenue
                        })
                );
                }
            } else
            {
                var lstParams = orders.GroupBy(s => s.CreatedAt.Value.Year)
                .Select(s => new { time = s.Key, revenue = s.Sum(o => o.TotalPrice) }).ToList();
                if (lstParams != null && lstParams.ToList().Count > 0)
                {
                    lstParams.ForEach(s => lstRevenues.Add(
                        new RevenueModel()
                        {
                            MonthYear = s.time.ToString(),
                            TotalRevenue = s.revenue
                        })
                );
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
                orders = orders.Where(s => (s.CreatedAt >= compareStartDate));
            }
            else
            {
                var compareStartDate = DateTime.Now.AddDays(-29);
                orders = orders.Where(s => (s.CreatedAt >= compareStartDate));
            }
            if (!string.IsNullOrEmpty(end))
            {
                var compareEndDate = Utility.GetNullableDate(end).Value.Date + new TimeSpan(23, 59, 59);
                orders = orders.Where(s => (s.CreatedAt <= compareEndDate));
            }
            else
            {
                var compareEndDate = DateTime.Now;
                orders = orders.Where(s => (s.CreatedAt <= compareEndDate));
            }

            var lstRevenues = new List<RevenuePieChartModel>();
            orders = orders.OrderBy(s => s.CreatedAt);
            var orderDetails = orders.SelectMany(s => s.OrderDetails).ToList();
            var totalRevenue = orderDetails.Sum(s => (s.UnitPrice * s.Quantity));
            if (orders != null && orders.ToList().Count > 0)
            {
                lstRevenues = orders.SelectMany(s => s.OrderDetails).GroupBy(s => s.FlowerCode).Select(cl => new RevenuePieChartModel
                {
                    FlowerName = cl.FirstOrDefault().Flower.Name,
                    FlowerRevenueRate = cl.Sum(c => (c.UnitPrice * c.Quantity)) * 100 / totalRevenue
                }).ToList();
            }

            var itemRevenue = new RevenuePieChartModel()
            {
                FlowerName = "Remaining",
                FlowerRevenueRate = lstRevenues.OrderByDescending(s => s.FlowerRevenueRate)
                .Skip(4).Sum(c => c.FlowerRevenueRate)
            };

            lstRevenues = lstRevenues.OrderByDescending(s => s.FlowerRevenueRate).Take(4).ToList();
            lstRevenues.Add(itemRevenue);

            return lstRevenues;
        }

        public string AdminUpdateStatus(Order item, string orderStatus)
        {
            item.Status = (OrderStatus)Enum.Parse(typeof(OrderStatus), orderStatus);
            item.UpdatedAt = DateTime.Now;
            item.UpdatedBy = userService.GetCurrentUserName();
            DbContext.Orders.AddOrUpdate(item);
            DbContext.SaveChanges();
            return orderStatus;
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
