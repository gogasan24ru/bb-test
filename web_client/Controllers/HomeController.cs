using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using web_client.Models;

namespace web_client.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var result = false;
            string Details = "default, no errors";
            try
            {
                result = (new ICUser()).TestConnection();
            }
            catch (Exception e)
            {
                Details = e.Message;
            }

            ViewData["WCFTestResult"] = ((result?"Success":"Failure") + ": " + Details+".");
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Logout()
        {
            var model = new ICUser();
            model.Logout(Session["SessionKey"] as string);
            
            return Redirect("/");
        }

        public ActionResult List()
        {
            throw new NotImplementedException();
        }

        public ActionResult Login()
        {
            //throw new NotImplementedException();
            return View();
        }

        public ActionResult Register()
        {
            throw new NotImplementedException();
        }

        public ActionResult LoginAction(LoginModel data)
        {
            var model = new ICUser();
//            model.
            throw new NotImplementedException();
            return Redirect("/");
        }
    }
}