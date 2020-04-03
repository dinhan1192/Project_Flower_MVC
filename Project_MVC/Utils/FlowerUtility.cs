using Microsoft.AspNet.Identity.Owin;
using Project_MVC.Models;
using Project_MVC.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project_MVC.Utils
{
    public static class FlowerUtility
    {
        private static MyDbContext _db;
        public static MyDbContext DbContext
        {
            get { return _db ?? HttpContext.Current.GetOwinContext().Get<MyDbContext>(); }
            set { _db = value; }
        }

        private static IUserService userService;

        static FlowerUtility()
        {
            userService = new UserService();
        }

        public static readonly CultureInfo UnitedStates =
        CultureInfo.GetCultureInfo("en-US");

        public static List<Flower> GetProducts(List<OrderDetail> orderDetails)
        {
            var lstProduct = new List<Flower>();
            foreach (var item in orderDetails)
            {
                var product = DbContext.Flowers.Find(item.FlowerCode);
                lstProduct.Add(product);
            }

            return lstProduct;
        }

        public static string GetFlowerName(string code)
        {
            var flower = DbContext.Flowers.Find(code);
            return flower.Name;
        }

        public static string GetFlowerImageUrl(string flowerCode)
        {
            var flower = DbContext.Flowers.Find(flowerCode);
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

        public static int GetReviews(string code)
        {
            var ratingCount = DbContext.RatingCounts.Count();
            if(ratingCount == null || ratingCount == 0)
            {
                return DbContext.RatingFlowers.Where(s => s.FlowerCode == code).Count();
            }

            return ratingCount;
        }

        [Authorize]
        public static ShoppingCart GetShoppingCart()
        {
            // lấy thông tin giỏ hàng ra.
            if (!(HttpContext.Current.Session[Constant.ShoppingCart] is ShoppingCart sc))
            {
                sc = new ShoppingCart();
            }
            return sc;
        }

        [Authorize]
        public static void ClearCart()
        {
            HttpContext.Current.Session.Remove(Constant.ShoppingCart);
            HttpContext.Current.Session.Remove(Constant.CurrentOrder);
        }

        [Authorize]
        public static Order GetCurrentOrder()
        {
            // lấy thông tin giỏ hàng ra.
            if (!(HttpContext.Current.Session[Constant.CurrentOrder] is Order or))
            {
                or = new Order();
            }
            return or;
        }

        public static bool CheckCurrentUserEmailConfirmed()
        {
            var userId = userService.GetCurrentUserId();

            var user = DbContext.Users.FirstOrDefault(u => u.Id == userId);
            return user.EmailConfirmed;
        }
    }
}