using Microsoft.AspNet.Identity.Owin;
using Project_MVC.Models;
using Project_MVC.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
            var ratingCount = DbContext.RatingCounts.Where(s => s.Code == code).FirstOrDefault();
            if(ratingCount == null)
            {
                return DbContext.RatingFlowers.Where(s => s.FlowerCode == code).Count();
            }

            return ratingCount.NumberOfRating;
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

        #region Drop Down List

        // Get the value of the description attribute if the   
        // enum has one, otherwise use the value.  
        public static string GetDescription<TEnum>(this TEnum value)
        {
            var fi = value.GetType().GetField(value.ToString());

            if (fi != null)
            {
                var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attributes.Length > 0)
                {
                    return attributes[0].Description;
                }
            }

            return value.ToString();
        }

        /// <summary>
        /// Build a select list for an enum
        /// </summary>
        public static SelectList SelectListFor<T>() where T : struct
        {
            Type t = typeof(T);
            return !t.IsEnum ? null
                             : new SelectList(BuildSelectListItems(t), "Value", "Text");
        }

        /// <summary>
        /// Build a select list for an enum with a particular value selected 
        /// </summary>
        public static SelectList SelectListFor<T>(T selected) where T : struct
        {
            Type t = typeof(T);
            return !t.IsEnum ? null
                             : new SelectList(BuildSelectListItems(t), "Text", "Value", selected.ToString());
        }

        private static IEnumerable<SelectListItem> BuildSelectListItems(Type t)
        {
            return Enum.GetValues(t)
                       .Cast<Enum>()
                       .Select(e => new SelectListItem { Value = e.ToString(), Text = e.GetDescription() });
        }

        #endregion
    }
}