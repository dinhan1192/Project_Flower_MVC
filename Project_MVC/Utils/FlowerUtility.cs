using Project_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_MVC.Utils
{
    public static class FlowerUtility
    {
        private static MyDbContext db = new MyDbContext();

        public static List<Flower> GetProducts(List<OrderDetail> orderDetails)
        {
            var lstProduct = new List<Flower>();
            foreach (var item in orderDetails)
            {
                var product = db.Flowers.Find(item.FlowerCode);
                lstProduct.Add(product);
            }

            return lstProduct;
        }

        public static string GetProductName(string code)
        {
            var flower = db.Flowers.Find(code);
            return flower.Name;
        }
    }
}