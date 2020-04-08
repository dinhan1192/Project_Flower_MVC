using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Project_MVC.App_Start;
using Project_MVC.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using System.Web.Routing;

namespace Project_MVC.Models
{
    public class CustomAuthorization : FilterAttribute, IAuthenticationFilter
    {
        //string clientRole = Constant.User; // can be taken from resource file or config file
        //string adminRole = Constant.Admin; // can be taken from resource file or config file

        //string currentArea = "";

        //private ApplicationUserManager _userManager;
        //public ApplicationUserManager UserManager
        //{
        //    get
        //    {
        //        return _userManager ?? HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
        //    }
        //    private set
        //    {
        //        _userManager = value;
        //    }
        //}

        //private IUserService userService;

        //public CustomAuthorization()
        //{
        //    userService = new UserService();
        //    //var roleStore = new RoleStore<AppRole>(DbContext);
        //    //roleManager = new RoleManager<AppRole>(roleStore);
        //}

        //public void OnAuthentication(AuthenticationContext context)
        //{
        //    if (context.HttpContext.User.Identity.IsAuthenticated)
        //    {
        //        var lstRoleName = UserManager.GetRoles(userService.GetCurrentUserId());
        //        string roleName = null;
        //        roleName = roleName ?? (lstRoleName.Contains(Constant.Admin) ? Constant.Admin : null);
        //        roleName = roleName ?? (lstRoleName.Contains(Constant.Employee) ? Constant.Employee : null);

        //        switch (roleName)
        //        {
        //            case Constant.Admin:
        //                context.Result = new HttpUnauthorizedResult();
        //                currentArea = Constant.Admin;
        //                break;
        //            case Constant.Employee:
        //                context.Result = new HttpUnauthorizedResult();
        //                currentArea = Constant.Employee;
        //                break;
        //            default:
        //                context.Result = new HttpUnauthorizedResult();
        //                currentArea = Constant.User;
        //                break;
        //        }
        //        //if (context.HttpContext.User.IsInRole(clientRole))
        //        //{
        //        //    context.Result = new HttpUnauthorizedResult();
        //        //    currentArea = Constant.User;
        //        //}
        //        //if (context.HttpContext.User.IsInRole(adminRole))
        //        //{
        //        //    context.Result = new HttpUnauthorizedResult();
        //        //    currentArea = Constant.Admin;
        //        //}
        //    }
        //    //else
        //    //{
        //    //    if (area.ToString().Equals(Constant.User))
        //    //    {
        //    //        context.Result = new HttpUnauthorizedResult();
        //    //        currentArea = Constant.User;
        //    //    }
        //    //    else if (area.ToString().Equals(Constant.Admin))
        //    //    {
        //    //        context.Result = new HttpUnauthorizedResult();
        //    //        currentArea = Constant.Admin;
        //    //    }
        //    //}
        //}

        //public void OnAuthenticationChallenge(AuthenticationChallengeContext context)
        //{
        //    if (context.Result == null)
        //    {
        //        context.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary(
        //     new { area = Constant.User, controller = "Accounts", action = "Login", returnUrl = context.HttpContext.Request.RawUrl }));
        //    }
        //    if (context.Result is HttpUnauthorizedResult)
        //    {
        //        if ((currentArea.Equals(Constant.User)))
        //        {
        //            context.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary(
        //     new { area = Constant.User, controller = "Accounts", action = "Login", returnUrl = context.HttpContext.Request.RawUrl }));
        //        }
        //        else if (currentArea.Equals(Constant.Employee))
        //        {
        //            context.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary(
        //    new { area = Constant.Employee, controller = "Accounts", action = "LoginAsAdmin", returnUrl = context.HttpContext.Request.RawUrl }));
        //        } else if (currentArea.Equals(Constant.Admin))
        //        {
        //            context.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary(
        //   new { area = Constant.Admin, controller = "Accounts", action = "LoginAsAdmin", returnUrl = context.HttpContext.Request.RawUrl }));
        //        }
        //    }
        //}

        string superAdminRole = Constant.Admin; // can be taken from resource file or config file
        string adminRole = Constant.Employee; // can be taken from resource file or config file

        public void OnAuthentication(AuthenticationContext context)
        {
            if (context.HttpContext.User.Identity.IsAuthenticated &&
               (context.HttpContext.User.IsInRole(superAdminRole)
                || context.HttpContext.User.IsInRole(adminRole)))
            {
                // do nothing
            }
            else
            {
                context.Result = new HttpUnauthorizedResult(); // mark unauthorized
            }
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext context)
        {
            if (context.Result == null || context.Result is HttpUnauthorizedResult)
            {
                context.Result = new RedirectToRouteResult("Default",
                    new System.Web.Routing.RouteValueDictionary{
                        {"controller", "Accounts"},
                        {"action", "LoginAsAdmin"},
                        {"returnUrl", context.HttpContext.Request.RawUrl}
                    });
            }
        }
    }
}