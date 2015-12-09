using DashBoard.Common;
using DashBoard.Web.Areas.HealthIndex.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DashBoard.Web.Areas.HealthIndex.Controllers
{
    /// <summary>
    /// 平台健康指数控制器
    /// 作者：刘静
    /// 修改时间：2015-11-30
    /// </summary>
    public class MaxTradeController : Controller
    {
        //
        // GET: /HealthIndex/MaxTrade/
        /// <summary>
        /// 显示平台健康指数
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// 配置平台健康指数
        /// </summary>
        /// <returns></returns>
        public ActionResult IndexManager()
        {
            HealthManagers config = new HealthManagers();
            config = IndexManagers.ReadConfig();
            ViewBag.MaxDay = config.MaxDayOrder;
            ViewBag.MaxMinute = config.MaxMinuteOrder;
            ViewBag.MaxSecond = config.MaxSecondOrder;
            ViewBag.LimitMinOrder = config.LimitMinOrder;
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public RedirectResult ChangeConfig()
        {
            string MaxDay = HttpContext.Request.Form["MaxDay"];
            string MaxMinute = HttpContext.Request.Form["MaxMinute"];
            string MaxSecond = HttpContext.Request.Form["MaxSecond"];
            string LimitMinOrder = HttpContext.Request.Form["LimitMinOrder"];
            IndexManagers.SaveConfig(MaxDay, MaxMinute, MaxSecond, LimitMinOrder);
            //...
            return new RedirectResult("/dashboard/HealthIndex/MaxTrade/IndexManager");

        }
    }
}
