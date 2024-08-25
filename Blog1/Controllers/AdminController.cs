using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blog1.Controllers
{
    public class AdminController : Controller
    {
        
        public ActionResult Index()
        {
            return View();
        }
    }
}