﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Project_MVC.Models;
using Project_MVC.Services;

namespace Project_MVC.Controllers
{
    public class OrdersController : Controller
    {
        private IOrderService mySQLOrderService;
        private IUserService userService;

        // GET: Orders
        public OrdersController()
        {
            mySQLOrderService = new MySQLOrderService();
            userService = new UserService();
        }

        public ActionResult Index(string sortOrder, string searchString, string currentFilter, int? page)
        {

            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            // lúc đầu vừa vào thì sortOrder là null, cho nên gán NameSortParm = name_desc
            // Ấn vào link Full name thì lúc đó NameSortParm có giá trị là name_desc, sortOrder trên view được gán = NameSortParm cho nên sortOrder != null
            // và NameSortParm = ""
            // Ấn tiếp vào link Full Name thì sortOrder = "" cho nên NameSortParm = name_desc
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var orders = mySQLOrderService.GetList();

            if (!String.IsNullOrEmpty(searchString))
            {
                orders = orders.Where(s => (!string.IsNullOrEmpty(s.ShipName) && s.ShipName.Contains(searchString)) || (!string.IsNullOrEmpty(s.CreatedBy) && s.CreatedBy.Contains(searchString)));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    orders = orders.OrderByDescending(s => s.ShipName);
                    break;
                case "Date":
                    orders = orders.OrderBy(s => s.UpdatedAt);
                    break;
                case "date_desc":
                    orders = orders.OrderByDescending(s => s.UpdatedAt);
                    break;
                default:
                    orders = orders.OrderBy(s => s.ShipName);
                    break;
            }


            int pageSize = Constant.PageSize;
            int pageNumber = (page ?? 1);
            ThisPage thisPage = new ThisPage()
            {
                CurrentPage = pageNumber,
                TotalPage = Math.Ceiling((double)orders.Count() / pageSize)
            };
            ViewBag.Page = thisPage;
            // nếu page == null thì lấy giá trị là 1, nếu không thì giá trị là page
            //return View(students.ToList().ToPagedList(pageNumber, pageSize));
            //var nl = mySQLOrderService.GetList().ToList();
            return View(orders.Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToList());
        }

        public ActionResult IndexCustomer(string sortOrder, string searchString, string currentFilter, int? page)
        {

            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            // lúc đầu vừa vào thì sortOrder là null, cho nên gán NameSortParm = name_desc
            // Ấn vào link Full name thì lúc đó NameSortParm có giá trị là name_desc, sortOrder trên view được gán = NameSortParm cho nên sortOrder != null
            // và NameSortParm = ""
            // Ấn tiếp vào link Full Name thì sortOrder = "" cho nên NameSortParm = name_desc
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var orders = mySQLOrderService.GetList().Where(s => s.UserId == userService.GetCurrentUserId() && s.Status == Order.OrderStatus.Pending);

            if (!String.IsNullOrEmpty(searchString))
            {
                orders = orders.Where(s => (!string.IsNullOrEmpty(s.ShipName) && s.ShipName.Contains(searchString)) || (!string.IsNullOrEmpty(s.CreatedBy) && s.CreatedBy.Contains(searchString)));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    orders = orders.OrderByDescending(s => s.ShipName);
                    break;
                case "Date":
                    orders = orders.OrderBy(s => s.UpdatedAt);
                    break;
                case "date_desc":
                    orders = orders.OrderByDescending(s => s.UpdatedAt);
                    break;
                default:
                    orders = orders.OrderBy(s => s.ShipName);
                    break;
            }


            //int pageSize = Constant.PageSize;
            //int pageNumber = (page ?? 1);
            //ThisPage thisPage = new ThisPage()
            //{
            //    CurrentPage = pageNumber,
            //    TotalPage = Math.Ceiling((double)orders.Count() / pageSize)
            //};
            //ViewBag.Page = thisPage;
            // nếu page == null thì lấy giá trị là 1, nếu không thì giá trị là page
            //return View(students.ToList().ToPagedList(pageNumber, pageSize));
            //var nl = mySQLOrderService.GetList().ToList();
            return View(orders);
        }

        // GET: Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = mySQLOrderService.Detail(id);
            if (order == null || order.IsDeleted())
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: Orders/Create
        //public ActionResult Create()
        //{
        //    ViewBag.UserId = new SelectList(db.AppUsers, "Id", "FirstName");
        //    return View();
        //}

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "Id,UserId,PaymentTypeId,ShipName,ShipAddress,ShipPhone,TotalPrice,CreatedAt,UpdatedAt,DeletedAt,CreatedBy,UpdatedBy,DeletedBy,Status")] Order order)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Orders.Add(order);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.UserId = new SelectList(db.AppUsers, "Id", "FirstName", order.UserId);
        //    return View(order);
        //}

        // GET: Orders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = mySQLOrderService.Detail(id);
            if (order == null || order.IsDeleted())
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserId,PaymentTypeId,ShipName,ShipAddress,ShipPhone,TotalPrice,CreatedAt,UpdatedAt,DeletedAt,CreatedBy,UpdatedBy,DeletedBy,Status")] Order order)
        {
            if (order == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var existOrder = mySQLOrderService.Detail(order.Id);
            if (existOrder == null || existOrder.IsDeleted())
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            if (mySQLOrderService.Update(existOrder, order, ModelState))
            {
                return RedirectToAction("Index");
            }

            return View(order);
        }

        // GET: Orders/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Order order = db.Orders.Find(id);
        //    if (order == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(order);
        //}

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var existProductCategory = mySQLOrderService.Detail(id);
            if (existProductCategory == null || existProductCategory.IsDeleted())
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            if (mySQLOrderService.Delete(existProductCategory, ModelState))
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                mySQLOrderService.DisposeDb();
            }
            base.Dispose(disposing);
        }
    }
}
