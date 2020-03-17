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

        public static string GetFlowerName(string code)
        {
            var flower = db.Flowers.Find(code);
            return flower.Name;
        }

        public static string GetFlowerImageUrl(string flowerCode)
        {
            var flower = db.Flowers.Find(flowerCode);
            if(flower != null)
            {
                var flowerImages = flower.FlowerImages;
                if(flowerImages != null && flowerImages.Count > 0)
                {
                    var flowerImage = flowerImages.OrderByDescending(s => s.CreatedAt).FirstOrDefault();
                    if(flowerImage != null)
                    {
                        return flowerImage.ImageUrl;
                    }
                }
            }

            return "";
        }

        public static ShoppingCart GetShoppingCart()
        {
            // lấy thông tin giỏ hàng ra.
            if (!(HttpContext.Current.Session[Constant.ShoppingCart] is ShoppingCart sc))
            {
                sc = new ShoppingCart();
            }
            return sc;
        }
    }
}