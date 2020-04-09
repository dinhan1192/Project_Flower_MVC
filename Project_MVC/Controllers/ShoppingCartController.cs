using Project_MVC.Models;
using Project_MVC.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using static Project_MVC.Models.Order;
using Project_MVC.Utils;
using System.Threading.Tasks;
using Project_MVC.App_Start;
using Microsoft.AspNet.Identity.Owin;

namespace Project_MVC.Controllers
{
    [Authorize]
    public class ShoppingCartController : Controller
    {
        private static string SHOPPING_CART_NAME = Constant.ShoppingCart;
        private static string CurrentOrder = Constant.ShoppingCart;
        private IUserService userService;
        private ICRUDService<Flower> mySQLFlowerService;
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        private MyDbContext db = new MyDbContext();
        public ShoppingCartController()
        {
            userService = new UserService();
            mySQLFlowerService = new MySQLFlowerService();
        }
        // GET: ShoppingCart
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddCart(string code, string strQuantity, string returnUrl2Previous, string type)
        {
            int quantity = Utility.GetInt(strQuantity);
            // Check số lượng có hợp lệ không?
            if (quantity <= 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Invalid Quantity");
            }
            // Check sản phẩm có hợp lệ không?
            var flower = db.Flowers.Find(code);
            if (flower == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, "Product's' not found");
            }

            // Lấy thông tin shopping cart từ session.
            var sc = LoadShoppingCart();
            // Thêm sản phẩm vào shopping cart.
            sc.AddCart(flower, quantity);
            // lưu thông tin cart vào session.
            SaveShoppingCart(sc);

            if (string.IsNullOrEmpty(type))
            {
                return RedirectToAction("ShowCart", new { returnUrl = Request.UrlReferrer.ToString() });
            }
            else
            {
                return Json(sc);
            }

            //if (string.IsNullOrEmpty(returnUrl))
            //{
            //    return RedirectToAction("ShowCart", new { categoryCode = flower.CategoryCode });
            //}
            //else
            //{
            //    return Json(sc);
            //}
        }

        [HttpPost]
        public ActionResult UpdatePerFlower(string code, string quantity, string returnCategoryCode)
        {
            // Check số lượng có hợp lệ không?
            int intQuantity = Utility.GetInt(quantity);
            if (intQuantity == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Invalid Quantity");
            }
            // Check sản phẩm có hợp lệ không?
            var flower = db.Flowers.Find(code);
            if (flower == null || flower.IsDeleted())
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, "Product's' not found");
            }
            // Lấy thông tin shopping cart từ session.
            var sc = LoadShoppingCart();
            // Thêm sản phẩm vào shopping cart.
            sc.UpdateFlowerInCart(flower, intQuantity);
            // lưu thông tin cart vào session.
            SaveShoppingCart(sc);
            return Json(intQuantity);
            //return RedirectToAction("ShowCart", new { categoryCode = returnCategoryCode });
            //return Redirect("ShowCart");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateCart(FormCollection frc)
        {
            // Check số lượng có hợp lệ không?
            int i = 0;
            string hidCategoryCode = Request["hidCategoryCode"];
            string[] strQuantities = frc.GetValues("quantity");
            if (strQuantities == null || strQuantities.Count() == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Invalid Quantity");
            }
            int[] intQuantities = Array.ConvertAll(strQuantities, s => int.TryParse(s, out i) ? i : 0);
            var check = intQuantities.Where(s => s <= 0).ToList();
            if (check.Count > 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Invalid Quantity");
            }
            // Check sản phẩm có hợp lệ không?
            //var product = db.Products.Find(productId);
            //if (product == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.NotFound, "Product's' not found");
            //}
            // Lấy thông tin shopping cart từ session.
            var sc = LoadShoppingCart();
            // Thêm sản phẩm vào shopping cart.
            sc.UpdateCart(intQuantities);
            // lưu thông tin cart vào session.
            SaveShoppingCart(sc);
            return RedirectToAction("ShowCart", new { categoryCode = hidCategoryCode });
            //return Redirect("ShowCart");
        }

        public ActionResult RemoveCart(string code, string returnUrl, string returnCategoryCode)
        {
            var flower = db.Flowers.Find(code);
            if (flower == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, "Flower's' not found");
            }
            // Lấy thông tin shopping cart từ session.
            var sc = LoadShoppingCart();
            // Thêm sản phẩm vào shopping cart.
            sc.RemoveFromCart(flower.Code);
            // lưu thông tin cart vào session.
            SaveShoppingCart(sc);

            if (string.IsNullOrEmpty(returnUrl))
            {
                return RedirectToAction("ShowCart", new { categoryCode = returnCategoryCode });
            }
            else if(returnUrl.Contains("LoadShoppingCartPartialView"))
            {
                return Redirect(Request.UrlReferrer.ToString());
            }
            else
            {
                return Redirect(returnUrl);
            }
        }

        public ActionResult ClearShoppingCart(string categoryCode, string returnUrl)
        {
            ClearCart();
            return RedirectToAction("ShowCart", new { categoryCode = categoryCode, returnUrl = returnUrl });
        }

        public ActionResult GetListOrders(int? page, string sortOrder, DateTime? start, DateTime? end)
        {
            ViewBag.CurrentSort = sortOrder;
            // lúc đầu vừa vào thì sortOrder là null, cho nên gán NameSortParm = name_desc
            // Ấn vào link Full name thì lúc đó NameSortParm có giá trị là name_desc, sortOrder trên view được gán = NameSortParm cho nên sortOrder != null
            // và NameSortParm = ""
            // Ấn tiếp vào link Full Name thì sortOrder = "" cho nên NameSortParm = name_desc
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            var ordersList = db.Orders.Where(s => (s.Status != Order.OrderStatus.Deleted));

            if (start != null && end != null)
            {
                TimeSpan tsEnd = new TimeSpan(23, 59, 59);
                TimeSpan tsStart = new TimeSpan(0, 0, 0);
                start = start.Value.Date + tsStart;
                end = end.Value.Date + tsEnd;
                ordersList = ordersList.Where(s => (s.CreatedAt >= start && s.CreatedAt <= end));
            }

            ordersList = ordersList.OrderBy(s => s.Id);

            switch (sortOrder)
            {
                case "Date":
                    ordersList = ordersList.OrderBy(s => s.CreatedAt);
                    break;
                case "date_desc":
                    ordersList = ordersList.OrderByDescending(s => s.CreatedAt);
                    break;
                default:
                    break;
            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            ViewBag.currentPage = pageNumber;
            ViewBag.totalPage = Math.Ceiling((double)ordersList.Count() / pageSize);

            // nếu page == null thì lấy giá trị là 1, nếu không thì giá trị là page
            //return View(students.ToList().ToPagedList(pageNumber, pageSize));
            return View(ordersList.Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToList());
            //if (page == null) page = 1;

            //int pageSize = 3;

            //var orders = db.Orders.ToList().OrderBy(x => x.Id);

            //int pageNumber = (page ?? 1);

            //if (page == null) page = 1;

            //int pageSize = 1;

            //int skip = (int)(page - 1) * pageSize;

            //var orders = db.Orders.ToList()
            //       .OrderBy(p => p.Id)
            //       .Skip(skip)
            //       .Take(pageSize);

            //var count = db.Products.Count();

            //var resultAsPagedList = new StaticPagedList<Order>(orders, (int)page, pageSize, count * pageSize);

            //return View(resultAsPagedList);
        }

        public ActionResult ShowCart(string categoryCode, string returnUrl)
        {
            ViewBag.shoppingCart = LoadShoppingCart();
            ViewBag.CategoryCode = categoryCode;
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        public ActionResult DisplayCartAfterCreateOrder(int? orderId)
        {
            var order = db.Orders.Find(orderId);
            if (order == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, "Not Found");
            }
            //ViewBag.ListFlowers = mySQLFlowerService.GetList().Where(s => order.OrderDetails.Select(p => p.FlowerCode).Contains(s.Code)).ToList();
            var lstFlowersModel = new List<FlowersInOrderModel>();
            foreach (var item in order.OrderDetails)
            {
                var flowerModel = new FlowersInOrderModel()
                {
                    FlowerName = mySQLFlowerService.Detail(item.FlowerCode).Name,
                    ImageUrl = mySQLFlowerService.Detail(item.FlowerCode).FlowerImages.OrderByDescending(s => s.CreatedAt).FirstOrDefault().ImageUrl,
                    Quantity = item.Quantity,
                    TotalPricePerFlower = item.Quantity * item.UnitPrice
                };
                lstFlowersModel.Add(flowerModel);
            }
            ViewBag.ListFlowersInOrder = lstFlowersModel;

            // Paypal
            //bool useSandbox = Convert.ToBoolean(ConfigurationManager.AppSettings["IsSandbox"]);
            //var paypal = new PayPalModel(useSandbox);
            ////Session[CurrentOrder] = order;
          
            //paypal.item_name = "Order of " + userService.GetCurrentUserName();
            //paypal.amount = order.TotalPrice.ToString();
            //var strParam = orderId + "," + userService.GetCurrentUserName() + "," + userService.GetCurrentUserId();
            //paypal.notify_url = paypal.notify_url + "?strParam=" + strParam;
            //paypal.@return = paypal.@return + "?orderId=" + orderId;
            //paypal.cancel_return = paypal.cancel_return + "?orderId=" + orderId;

            //ViewBag.Paypal = paypal;
            return View(order);
        }

        public ActionResult CreateOrder()
        {
            var shoppingCart = LoadShoppingCart();
            if (shoppingCart.GetCartItems().Count <= 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Bad request");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateOrder([Bind(Include = "ShipName,ShipPhone,ShipAddress,PaymentTypeId")]CartInformation cartInfo)
        {
            // load cart trong session.
            if (ModelState.IsValid)
            {
                var shoppingCart = LoadShoppingCart();
                if (shoppingCart.GetCartItems().Count <= 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Bad request");
                }
                // chuyển thông tin shopping cart thành Order.
                var order = new Order
                {
                    TotalPrice = shoppingCart.GetTotalPrice(),
                    UserId = userService.GetCurrentUserId(),
                    // can sua Language
                    //Language = cartInfo.Language,
                    PaymentTypeId = (PaymentType)Enum.Parse(typeof(PaymentType), cartInfo.PaymentTypeId),
                    ShipName = cartInfo.ShipName,
                    ShipPhone = cartInfo.ShipPhone,
                    ShipAddress = cartInfo.ShipAddress,
                    OrderDetails = new List<OrderDetail>(),
                    CreatedBy = userService.GetCurrentUserName(),
                    UpdatedBy = userService.GetCurrentUserName()
                };
                // Tạo order detail từ cart item.
                foreach (var cartItem in shoppingCart.GetCartItems())
                {
                    var orderDetail = new OrderDetail()
                    {
                        FlowerCode = cartItem.Value.FlowerCode,
                        OrderId = order.Id,
                        Quantity = cartItem.Value.Quantity,
                        UnitPrice = cartItem.Value.Price,
                        Status = OrderDetail.OrderDetailStatus.NotDeleted
                    };
                    order.OrderDetails.Add(orderDetail);
                }
                db.Orders.Add(order);
                db.SaveChanges();
                ClearCart();
                var strHomeUrl = Constant.WebURL + @"ShoppingCart/DisplayCartAfterCreateOrder?orderId=" + order.Id;
                await UserManager.SendEmailAsync(userService.GetCurrentUserId(),
                    "Congratulation: You have successfully created order!",
                    "Thank for choosing our flowers! Please click <a href=\"" + strHomeUrl + "\">here</a> to have a look at your cart!");
                //// lưu vào database.
                //var transaction = db.Database.BeginTransaction();
                //try
                //{

                //    transaction.Commit();
                //}
                //catch (Exception e)
                //{
                //    Console.WriteLine(e);
                //    transaction.Rollback();
                //}
                return RedirectToAction("DisplayCartAfterCreateOrder", new { orderId = order.Id });
            }

            return View(cartInfo);
        }

        public ActionResult LoadShoppingCartPartialView()
        {
            LoadShoppingCart();
            return PartialView("_LoginPartial");
        }


        private void ClearCart()
        {
            Session.Remove(SHOPPING_CART_NAME);
        }

        /**
         * Tham số nhận vào là một đối tượng shopping cart.
         * Hàm sẽ lưu đối tượng vào session với key được define từ trước.
         */
        private void SaveShoppingCart(ShoppingCart shoppingCart)
        {
            Session[SHOPPING_CART_NAME] = shoppingCart;
        }

        /**
         * Lấy thông tin shopping cart từ trong session ra. Trong trường hợp không tồn tại
         * trong session thì tạo mới đối tượng shopping cart.
         */
        private ShoppingCart LoadShoppingCart()
        {
            // lấy thông tin giỏ hàng ra.
            if (!(Session[SHOPPING_CART_NAME] is ShoppingCart sc))
            {
                sc = new ShoppingCart();
            }
            return sc;
        }

        public ActionResult GetListProductsInOrder(int id)
        {
            var lstOrderDetail = db.OrderDetails.Where(s => s.OrderId == id).ToList();
            var lstProduct = new List<Flower>();
            double totalPrice = 0;
            foreach (var item in lstOrderDetail)
            {
                var flower = db.Flowers.Find(item.FlowerCode);
                lstProduct.Add(flower);
                totalPrice += item.Quantity * flower.Price;
            }

            ViewBag.totalPrice = totalPrice;

            return View(lstProduct);
        }

        #region Paypal

        public ActionResult RedirectFromPaypal(string orderId)
        {
            var messageView = "Successfully Paid!";
            return RedirectToAction("PaidResult", new { message = messageView, orderIdView = orderId });
        }

        public ActionResult PaidResult(string message, string orderIdView)
        {
            ViewBag.DisplayMsg = message;
            ViewBag.OrderId = orderIdView;
            return View();
        }

        public ActionResult CancelFromPaypal(string orderId)
        {
            var messageView = "Cancel from Payment!";
            return RedirectToAction("PaidResult", new { message = messageView, orderIdView = orderId });
        }

        public ActionResult NotifyFromPaypal()
        {
            return RedirectToAction("Receive", "IPN");
        }

        public ActionResult ValidateCommand(int orderId)
        {
            bool useSandbox = Convert.ToBoolean(ConfigurationManager.AppSettings["IsSandbox"]);
            var paypal = new PayPalModel(useSandbox);

            var order = db.Orders.Find(orderId);
            Session[CurrentOrder] = order;

            //var lstPaypal = new List<string>();

            //foreach (var item in order.OrderDetails)
            //{
            //    var flowerName = FlowerUtility.GetFlowerName(item.FlowerCode);
            //    lstPaypal.Add(flowerName);
            //    //paypal.amount += "amount_" + itemCount.ToString() + cartItem.Product.Price;
            //    //paypal.quantity += "quantity_" + itemCount.ToString() + cartItem.Quantity;
            //    //paypal.shipping += "shipping_" + itemCount.ToString() + cartItem.Product.Price;
            //    //paypal.handling += "handling_" + itemCount.ToString() + "0";
            //}

            paypal.item_name = "Order of " + userService.GetCurrentUserName();
            paypal.amount = order.TotalPrice.ToString();
            var strParam = orderId + "," + userService.GetCurrentUserName() + "," + userService.GetCurrentUserId();
            paypal.notify_url = paypal.notify_url + "?strParam=" + strParam;
            paypal.@return = paypal.@return + "?orderId=" + orderId;
            paypal.cancel_return = paypal.cancel_return + "?orderId=" + orderId;
            //paypal.notify_url = "https://localhost:44320/IPN/Receive";
            //paypal.actionURL = "https://www.sandbox.paypal.com/cgi-bin/webscr";
            //paypal.no_shipping = "1";
            //paypal.quantity = order.OrderDetails.Sum(s => s.Quantity).ToString();
            return Redirect(paypal.actionURL + "?cmd=" + paypal.cmd + "&business=" + paypal.business +
                "&no_shipping=" + paypal.no_shipping + "&return=" + paypal.@return + "&cancel_return=" + paypal.cancel_return +
                "&notify_url=" + paypal.notify_url + "&currency_code" + paypal.currency_code + "&item_name=" + paypal.item_name +
                "&amount=" + paypal.amount);
        }

        #endregion
    }
}