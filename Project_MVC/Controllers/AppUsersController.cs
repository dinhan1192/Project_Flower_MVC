using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Project_MVC.App_Start;
using Project_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Net;
using Project_MVC.Services;
using Project_MVC.Utils;
using System.Data.Entity.Migrations;
using System.Web.Security;
using Microsoft.Ajax.Utilities;

namespace Project_MVC.Controllers
{
    [Authorize(Roles = Constant.Admin)]
    public class AppUsersController : Controller
    {
        // GET: AppUsers
        private MyDbContext _db;
        public MyDbContext DbContext
        {
            get { return _db ?? HttpContext.GetOwinContext().Get<MyDbContext>(); }
            set { _db = value; }
        }
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

        private IUserService userService;

        public AppUsersController()
        {
            userService = new UserService();
            //var roleStore = new RoleStore<AppRole>(DbContext);
            //roleManager = new RoleManager<AppRole>(roleStore);
        }
        //public AppUsersController()
        //{
        //    DbContext = new MyDbContext();
        //    UserStore<AppUser> userStore = new UserStore<AppUser>(DbContext);
        //    UserManager = new UserManager<AppUser>(userStore);
        //}

        public ActionResult CheckUserInRoles()
        {
            if (RolesUtil.IsInAnyRole(User, new string[] { Constant.Admin }))
            {
                return Json(true);
            }

            return Json(false);
        }

        [HttpGet]
        [CustomAuthorization]
        public PartialViewResult AddRolePopup(string Id)
        {
            ViewBag.userIds = Id;
            var roles = DbContext.IdentityRoles.ToList();
            return PartialView("PopupForAddRole", roles);
        }

        //public ActionResult AddUsers2Roles(string[] ids, string[] roleNames)
        //{
        //    foreach (var id in ids)
        //    {
        //        userManager.AddToRoles(id, roleNames);
        //    }
        //    return View();
        //}

        [HttpPost]
        [CustomAuthorization]
        public ActionResult AddUsers2Roles(string Id, string RoleName)
        {
            var arrUserIds = Id.Split(',');
            var arrRoleNames = RoleName.Split(',');
            foreach (var id in arrUserIds)
            {
                foreach (var roleName in arrRoleNames)
                {
                    UserManager.AddToRole(id, roleName);
                }
                //UserManager.AddToRoles(id, arrRoleNames);
            }

            //try
            //{
            //    var arrUserIds = Id.Split(',');
            //    var arrRoleNames = RoleName.Split(',');
            //    foreach (var id in arrUserIds)
            //    {
            //        //foreach(var roleName in arrRoleNames)
            //        //{
            //        //    UserManager.AddToRole(id, roleName);
            //        //}
            //        UserManager.AddToRoles(id, arrRoleNames);
            //    }
            //}
            //catch (Exception e)
            //{
            //    Debug.WriteLine(e);
            //    throw;
            //}

            return Json(true);
        }
        // GET: AppUsers
        [CustomAuthorization]
        public ActionResult Index(int? page)
        {
            var users = DbContext.Users.Where(s => s.Status == AppUser.UserStatus.NotDeleted).ToList();
            int pageSize = Constant.PageSize;
            int pageNumber = (page ?? 1);
            ThisPage thisPage = new ThisPage()
            {
                CurrentPage = pageNumber,
                TotalPage = Math.Ceiling((double)users.Count() / pageSize)
            };
            ViewBag.Page = thisPage;
            return View(users.Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToList());
        }

        [CustomAuthorization]
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = DbContext.Users.Find(id);
            var roles = user.Roles;
            var lstRoleNames = new List<string>();
            roles.ForEach(s => lstRoleNames.Add(DbContext.IdentityRoles.Find(s.RoleId).Name));
            ViewBag.Roles = lstRoleNames;
            if (user == null || user.IsDeleted())
            {
                return HttpNotFound();
            }
            return View(user);
        }

        [CustomAuthorization]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var existUser = DbContext.Users.Find(id);
            if (existUser == null || existUser.IsDeleted())
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            if (ModelState.IsValid)
            {
                existUser.Status = AppUser.UserStatus.Deleted;
                existUser.DeletedAt = DateTime.Now;
                existUser.DeletedBy = userService.GetCurrentUserName();
                DbContext.Users.AddOrUpdate(existUser);
                DbContext.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}