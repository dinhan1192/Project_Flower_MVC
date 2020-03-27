using Microsoft.AspNet.Identity.Owin;
using Project_MVC.App_Start;
using Project_MVC.Services;
using Project_MVC.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Project_MVC.Controllers
{
    public class IPNController : Controller
    {
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
        private IOrderService orderService;
        public IPNController()
        {
            userService = new UserService();
            orderService = new MySQLOrderService();
        }

        //[ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<HttpStatusCodeResult> Receive(string strParam)
        {
            //var order = orderService.Detail(Utility.GetNullableInt(orderId));
            //order.ShipName = Request.Url.ToString();
            //orderService.UpdateStatus(order);
            //UserManager.SendEmailAsync(userService.GetCurrentUserId(),
            //    "Congratulation: You have successfully paid!",
            //    "Thank for buying our flowers! Please click <a href=\"" + Url.Action("Index", "Home") + "\">here</a> to go to our Homepage!");
            string[] arrParams = strParam.Split(',');
            var orderId = arrParams[0];
            var userName = arrParams[1];
            var userId = arrParams[2];

            //Store the IPN received from PayPal
            LogRequest(Request);

            //Fire and forget verification task
            //Task.Run(() => VerifyTask(Request));
            await VerifyTask(Request, orderId, userName, userId);

            //Reply back a 200 code
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        private async Task VerifyTask(HttpRequestBase ipnRequest, string orderId, string userName, string userId)
        {
            var verificationResponse = string.Empty;
            //var orderId = ipnRequest.Params.Get("orderId");
            //var userName = ipnRequest.Params.Get("userName");
            //var userId = ipnRequest.Params.Get("userId");

            try
            {
                var verificationRequest = (HttpWebRequest)WebRequest.Create("https://www.sandbox.paypal.com/cgi-bin/webscr");

                //Set values for the verification request
                verificationRequest.Method = "POST";
                verificationRequest.ContentType = "application/x-www-form-urlencoded";
                var param = Request.BinaryRead(ipnRequest.ContentLength);
                var strRequest = Encoding.ASCII.GetString(param);

                //Add cmd=_notify-validate to the payload
                strRequest = "cmd=_notify-validate&" + strRequest;
                verificationRequest.ContentLength = strRequest.Length;

                //Attach payload to the verification request
                var streamOut = new StreamWriter(verificationRequest.GetRequestStream(), Encoding.ASCII);
                streamOut.Write(strRequest);
                streamOut.Close();

                //Send the request to PayPal and get the response
                var streamIn = new StreamReader(verificationRequest.GetResponse().GetResponseStream());
                verificationResponse = streamIn.ReadToEnd();
                streamIn.Close();

            }
            catch (Exception exception)
            {
                //Capture exception for manual investigation
            }

            await ProcessVerificationResponse(verificationResponse, orderId, userName, userId);
        }


        private void LogRequest(HttpRequestBase request)
        {
            // Persist the request values into a database or temporary data store
        }

        private async Task ProcessVerificationResponse(string verificationResponse, string orderId, string userName, string userId)
        {
            if (verificationResponse.Equals("VERIFIED"))
            {
                // check that Payment_status=Completed
                // check that Txn_id has not been previously processed
                // check that Receiver_email is your Primary PayPal email
                // check that Payment_amount/Payment_currency are correct
                // process payment

                var order = orderService.Detail(Utility.GetNullableInt(orderId));
                orderService.UpdateStatus(order, userName);
                var strHomeUrl = @"https://flowermvcatttversion02.azurewebsites.net//Home/Index";
                await UserManager.SendEmailAsync(userId,
                    "Congratulation: You have successfully paid!",
                    "Thank for buying our flowers! Please click <a href=\"" + strHomeUrl + "\">here</a> to go to our Homepage!");

            }
            else if (verificationResponse.Equals("INVALID"))
            {
                //Log for manual investigation
            }
            else
            {
                //Log error
            }
        }
    }
}