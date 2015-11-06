using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DashBoard.Web.Areas.CustomerData.Controllers
{
    public class CustomerController : Controller
    {
        //
        // GET: /CustomerData/Customer/

        public ActionResult Index()
        {
            return View();
        }

    }
}
