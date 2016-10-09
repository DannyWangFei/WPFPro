using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Web;
using System.Web.Mvc;
using WPFPro.BLL;
using WPFPro.Models;

namespace WPFPro.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to ASP.NET MVC!";

            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Product()
        {
            return View();
        }

        public ActionResult Contactus()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult ajaxSubmitContactus(string contactName, string contactEmail, string mobilePhone, string note)
        {            
            if (contactEmail.Trim() == "")
            {
                return View();
            }
            BLLContactUs contact = new BLLContactUs();
            bool result = contact.InsertContactus(contactName, contactEmail, mobilePhone, note);
            if (result)
            { return Json(new { IsSuccess = true }); }
            else
            { return Json(new { IsSuccess = false }); }
        }
    }
}
