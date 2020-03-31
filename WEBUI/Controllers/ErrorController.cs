using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WEBUI.Controllers
{
    public class ErrorController : Controller
    {

        public ActionResult DataNotFound(string message)
        {
            Response.StatusCode = 404;
            ViewBag.MyMessage = message;
            return View();
        }
        public ActionResult DataCouldNotBeDeleted(string message)
        {
            Response.StatusCode = 404;
            ViewBag.MyMessage = message;
            return View();
        }
    }
}