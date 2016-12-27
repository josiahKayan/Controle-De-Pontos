using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Apassos.Controllers
{
    public class HomeController : Controller
    {
        public String Index()
        {
            ViewBag.Message = "Welcome to ASP.NET MVC!";

            return "Teste de login";
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
