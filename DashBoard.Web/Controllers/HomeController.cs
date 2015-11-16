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
            //object result = WebServiceHelper.InvokeAndCallWebService(ConfigurationManager.AppSettings["ValidationUrl"], "WSService", "CanAccessDashboard");
            //result.ToString();
            //WSService.WSServiceSoapClient ws = new WSService.WSServiceSoapClient();
            //int result = ws.CanAccessDashboard();
            ViewBag.Title = "国泰君安量化交易情况统计分析系统";
            //ViewBag.Title = result.ToString();
            return View();
        }
        
    }
}
