using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UsersEdit.Controllers
{
    public class ErrorController : Controller
    {
        [HttpGet]
        public ActionResult HttpError()
        {
            HttpContext.Response.StatusCode = 400;
            return View();
        }

        [HttpGet]
        public ActionResult ApplicationError()
        {
            HttpContext.Response.StatusCode = 500;
            return View();
        }

	}
}