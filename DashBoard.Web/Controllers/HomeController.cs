using Dashboard.Common;
using Dashboard.Logic;
using DashBoard.Logic;
using DashBoard.Web.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DashBoard.Web.Controllers
{
    /// <summary>
    /// 主控制器，通过该控制器访问所有页面
    /// 作者：刘静
    /// 修改时间：2015-11-30
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// 框架页面，通过该页面进行排版
        /// </summary>
        /// <returns></returns>
        [UserAuthorize]
        public ActionResult Index()
        {
            string login = Request["sessionid"];
            ViewBag.Title = "国泰君安量化交易情况统计分析系统";
            return View();
            //return Content("<script >alert('" + login + "');</script >", "text/html");
        }

        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        [UserAuthorize]
        public ActionResult HomePage()
        {
            ViewBag.Title = "国泰君安量化交易情况统计分析系统";
            return View();
        }

        /// <summary>
        /// 页面顶部，logo等
        /// </summary>
        /// <returns></returns>
        [UserAuthorize]
        public ActionResult Top()
        {
            return View();
        }

        /// <summary>
        /// 页面左侧导航栏
        /// </summary>
        /// <returns></returns>
        [UserAuthorize]
        public ActionResult Left()
        {
            return View();
        }

        /// <summary>
        /// 主显示页面
        /// </summary>
        /// <returns></returns>
        [UserAuthorize]
        public ActionResult Right()
        {
            return View();
        }

        /// <summary>
        /// 登录错误页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Error()
        {
            return View();
        }
    }
}
