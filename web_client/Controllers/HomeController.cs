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
            return Redirect("/Home/ListUsers");
            //increase logout debug speed  

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
//            model.Logout(Session["SessionKey"] as string);
            model.Logout("SOMESTRING");

            return Redirect("/");
        }

        public ActionResult ListUsers(int page = 0)
        {
            if (page < 0) page = 0;
            var model = new ICUser();
            //            model.Logout(Session["SessionKey"] as string);
            var list = model.ListUsers("SOMESTRING",page);
            while((list.Count == 0)&& (page >= 0)) list = model.ListUsers("SOMESTRING", --page);
            //TODO max page count should be received from server 
            ViewData["page"] = page;
            return View(list);
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