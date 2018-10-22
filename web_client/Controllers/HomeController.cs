using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClassLibrary1;
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
            try
            {
                model.Logout(sessionKey);
            }
            catch (Exception e)
            {

                ViewData["Message"] = e.Message;
                return View("Error");

            }

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
                var maxPage = model.GetMaxPage(sessionKey)-1;
                if (page > maxPage) page = maxPage;
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
            //new User(){Age=10};//[Range(18, int.MaxValue)] not throwing exception, wtf.
            //assuming i should validate it myself.
            int errors = 0;

            if (data.ChangedFlag == false)
                return View();
            data.ChangedFlag = false;

            //if () errors++;
            
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

            if (!ModelState.IsValid)
            {
                string message="";
                foreach (var key in ViewData.ModelState.Keys)
                    foreach (ModelError error in ModelState[key].Errors)
                    {
                        message += key + " (" + (ModelState[key].Value.AttemptedValue) + "): " + error.ErrorMessage + "<br/>";
                    }

                ViewData["Message"] = ("Поля заполнены неправильно:<br/>"+message);
                return View("Error");
            }

            var model= new ICUser();
            try
            {
                model.Register(data);
            }
            catch (Exception e)
            {

                ViewData["Message"] = e.Message+ "<br/>Скорее всего ([Index(IsClustered=false,<b>IsUnique = true</b>,Order=0)]), пользователь с таким именем пользователя уже существует //TODO";
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