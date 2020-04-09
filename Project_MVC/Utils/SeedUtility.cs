using FizzWare.NBuilder;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity.Owin;
using Project_MVC.Models;
using Project_MVC.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using static Project_MVC.Models.Order;

namespace Project_MVC.Utils
{
    public static class SeedUtility
    {
        private static MyDbContext _db;
        public static MyDbContext DbContext
        {
            get { return _db ?? HttpContext.Current.GetOwinContext().Get<MyDbContext>(); }
            set { _db = value; }
        }

        private static IUserService userService = new UserService();
        private static ICRUDService<Flower> mySQLFlowerService = new MySQLFlowerService();

        public static void SeedRandomOrder(string type)
        {
            switch (type)
            {
                case Constant.SeedRandomOrders:
                    var customers = DbContext.Users.ToList();
                    var flowers = mySQLFlowerService.GetList().ToList();
                    var daysGenerator = new RandomGenerator();
                    var paymentTypeGenerator = new RandomGenerator();
                    var statusGenerator = new RandomGenerator();
                    var priceGenerator = new RandomGenerator();
                    var orderIdGenerator = new RandomGenerator();

                    var orders = Builder<Order>.CreateListOfSize(1000)
                        .All()
                            //.With(o => o.Id = null)
                            .With(o => o.UserId = Pick<AppUser>.RandomItemFrom(customers).Id)
                            .With(o => o.PaymentTypeId = (PaymentType)Enum.Parse(typeof(PaymentType), paymentTypeGenerator.Next(1, 3).ToString()))
                            //.With(o => o.CreatedAt = DateTime.Now.AddDays(-daysGenerator.Next(1, 9)))
                            .With(o => o.CreatedAt = DateTime.Now.AddDays(-daysGenerator.Next(1, 3650)))
                            //.With(o => o.CreatedAt = DateTime.Now.AddMonths(-daysGenerator.Next(1, 120)))
                            //.With(o => o.CreatedAt = DateTime.Now.AddYears(-daysGenerator.Next(1, 120)))
                            .With(o => o.UpdatedAt = o.CreatedAt.Value.AddDays(2))
                            .With(o => o.CreatedBy = userService.GetCurrentUserName())
                            .With(o => o.UpdatedBy = userService.GetCurrentUserName())
                            .With(o => o.ShipName = Faker.Name.FullName())
                            .With(o => o.ShipAddress = Faker.Address.StreetAddress())
                            .With(o => o.ShipPhone = Faker.Phone.Number())
                            //.With(o => o.Status = (OrderStatus)Enum.Parse(typeof(OrderStatus), statusGenerator.Next(1, 6).ToString()))
                            .With(o => o.Status = OrderStatus.Done)
                            .With(o => o.DeletedAt = null)
                            .With(o => o.DeletedBy = null)
                        //.With(o => o.TotalPrice = priceGenerator.Next(22, 500))
                        .Build();

                    //DbContext.Orders.AddRange(orders);
                    //DbContext.SaveChanges();

                    // Generate order items
                    var itemCountGenerator = new RandomGenerator();
                    var quantityGenerator = new RandomGenerator();

                    //orders = DbContext.Orders.Where(s => s.Status == OrderStatus.Done).ToList();
                    //foreach (var itemOrder in orders)
                    //{
                    //    var listOrderDetailsSeed = itemOrder.OrderDetails.ToList();
                    //    if (listOrderDetailsSeed == null || listOrderDetailsSeed.Count == 0)
                    //    {
                    //        var orderDetails = Builder<OrderDetail>.CreateListOfSize(itemCountGenerator.Next(1, 10))
                    //        .All()
                    //            .With(oi => oi.Id = null)
                    //            .With(oi => oi.Order = itemOrder)
                    //            .With(oi => oi.OrderId = itemOrder.Id)
                    //            .With(oi => oi.Flower = Pick<Flower>.RandomItemFrom(flowers))
                    //            .With(oi => oi.FlowerCode = oi.Flower.Code)
                    //            .With(oi => oi.Quantity = quantityGenerator.Next(1, 10))
                    //            .With(oi => oi.UnitPrice = oi.Flower.Price)
                    //        .Build();
                    //        DbContext.Orders.AddOrUpdate(itemOrder);
                    //        DbContext.SaveChanges();
                    //    }
                    //}

                    //var ordersTest = DbContext.Orders.Where(s => s.Status == OrderStatus.Done).ToList();
                    orders.ForEach(o =>
                    {
                        var orderDetails = Builder<OrderDetail>.CreateListOfSize(itemCountGenerator.Next(2, 8))
                        .All()
                            //.With(oi => oi.Id = null)
                            .With(oi => oi.Order = o)
                            .With(oi => oi.OrderId = o.Id)
                            .With(oi => oi.Flower = Pick<Flower>.RandomItemFrom(flowers))
                            .With(oi => oi.FlowerCode = oi.Flower.Code)
                            .With(oi => oi.Quantity = quantityGenerator.Next(1, 10))
                            .With(oi => oi.UnitPrice = oi.Flower.Price)
                        .Build();

                        o.TotalPrice = orderDetails.Sum(s => (s.UnitPrice * s.Quantity));
                        //DbContext.Orders.AddOrUpdate(o);
                        DbContext.OrderDetails.AddOrUpdate(oi => oi.Id, orderDetails.ToArray());
                        //DbContext.Orders.AddOrUpdate(o);
                    });
                    DbContext.Orders.AddOrUpdate(o => o.Id, orders.ToArray());
                    DbContext.SaveChanges();
                    //    //var orderDetails = Builder<OrderDetail>.CreateListOfSize(itemCountGenerator.Next(1, 10))
                    //    //    .All()
                    //    //        //.With(oi => oi.Id = )
                    //    //        .With(oi => oi.Order = o)
                    //    //        .With(oi => oi.OrderId = o.Id)
                    //    //        .With(oi => oi.Flower = Pick<Flower>.RandomItemFrom(flowers))
                    //    //        .With(oi => oi.FlowerCode = oi.Flower.Code)
                    //    //        .With(oi => oi.Quantity = quantityGenerator.Next(1, 10))
                    //    //        .With(oi => oi.UnitPrice = oi.Flower.Price)
                    //    //    .Build();
                    //    //DbContext.OrderDetails.AddOrUpdate(oi => oi.Id, orderDetails.ToArray());
                    //    //.GroupBy(s => s.FlowerCode).Select(s => s.FirstOrDefault())
                    //});

                    //orders = orders.ForEach(o => o.TotalPrice = o.OrderDetails.Sum(s => (s.UnitPrice * s.Quantity)));
                    //orders.ForEach(o => o.TotalPrice = o.OrderDetails.Sum(s => (s.UnitPrice * s.Quantity)));
                    //var compareDate = DateTime.Now.AddYears(-3);
                    //ordersTest = ordersTest.Where(s => DateTime.Compare(s.UpdatedAt.Value, compareDate) > 0).ToList();
                    //foreach (var item in orders)
                    //{
                    //    item.TotalPrice = item.OrderDetails.Sum(s => (s.UnitPrice * s.Quantity));
                    //}
                    break;
                case Constant.DeleteUnknownOrders:
                    var lstFlowers = mySQLFlowerService.GetList().ToList();
                    var lstOrders = DbContext.Orders.Where(s => s.Status != OrderStatus.Deleted).ToList();

                    lstOrders.ForEach(o =>
                    {
                        var listOrderDetails = o.OrderDetails.ToList();
                        if (listOrderDetails == null || listOrderDetails.Count == 0)
                        {
                            o.Status = OrderStatus.Deleted;
                            DbContext.Orders.AddOrUpdate(o);
                        }
                        else
                        {
                            var result = listOrderDetails.Where(p => lstFlowers.All(p2 => p2.Code != p.FlowerCode)).ToList();
                            if (result != null && result.Count > 0)
                            {
                                o.Status = OrderStatus.Deleted;
                                DbContext.Orders.AddOrUpdate(o);
                            }
                        }
                    });
                    //foreach (var item in lstOrders)
                    //{
                    //    var listOrderDetails = item.OrderDetails.ToList();
                    //    if (listOrderDetails == null || listOrderDetails.Count == 0)
                    //    {
                    //        item.Status = OrderStatus.Deleted;
                    //        DbContext.Orders.AddOrUpdate(item);
                    //    }
                    //    else
                    //    {
                    //        var result = listOrderDetails.Where(p => lstFlowers.All(p2 => p2.Code != p.FlowerCode)).ToList();
                    //        if (result != null && result.Count > 0)
                    //        {
                    //            item.Status = OrderStatus.Deleted;
                    //            DbContext.Orders.AddOrUpdate(item);
                    //        }
                    //    }
                    //}

                    DbContext.SaveChanges();
                    break;
                default:
                    break;
            }
        }
    }
}