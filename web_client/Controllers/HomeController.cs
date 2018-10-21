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
            //return Redirect("/Home/ListUsers");
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
            string sessionKey = (string)Session["SessionKey"] ?? "";
            model.Logout(sessionKey);

            Session["SessionKey"] = null;
            Session["Login"] = null;
            Session["LoggedIn"] = false;
            return Redirect("/");
        }

        public ActionResult ListUsers(int page = 0)
        {
            try
            {
                if (page < 0) page = 0;
                var model = new ICUser();
                string sessionKey = (string)Session["SessionKey"] ?? "";
                var list = model.ListUsers(sessionKey, page);
                while((list.Count == 0)&& (page >= 0)) list = model.ListUsers(sessionKey, --page);
                //TODO max page count should be received from server 
                ViewData["page"] = page;
                return View(list);
            }
            catch (Exception e)
            {
                ViewData["Message"] = e.Message;
                return View("Error");
            }
        }

        public ActionResult Login()
        {
            //throw new NotImplementedException();
            return View();
        }

        public ActionResult Register(ClassLibrary1.User data)
        {
            if(data.IsAuthenticated==false)
                return View();
            data.IsAuthenticated = false;

            //...
            try
            {
//                var model = new ICUser();
//                var list = model.Register
//                return View(list);
            }
            catch (Exception e)
            {
                ViewData["Message"] = e.Message;
                return View("Error");
            }

            return Redirect("/Home/Login");
            throw new NotImplementedException();
        }

        public ActionResult LoginAction(LoginModel data)
        {
            var model = new ICUser();
            try
            {
                Session["SessionKey"] = model.Login(data.Username, data.Password);
                Session["Login"] = data.Username;
                Session["LoggedIn"] = true;
            }
            catch (Exception e)
            {
                ViewData["Message"] = e.Message;
                return View("Error");
            }
//            throw new NotImplementedException();
            return Redirect("/");
        }
    }
}