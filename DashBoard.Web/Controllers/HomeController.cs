using Dashboard.Common;
using Dashboard.Logic;
using DashBoard.Logic;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DashBoard.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "国泰君安量化交易情况统计分析系统";
            return View();
        }
        public ActionResult HomePage()
        {
            ViewBag.Title = "国泰君安量化交易情况统计分析系统";
            return View();
        }
        public ActionResult Top()
        {
            return View();
        }
        public ActionResult Left()
        {
            return View();
        }
        public ActionResult Right()
        {
            return View();
        }
    }
}
