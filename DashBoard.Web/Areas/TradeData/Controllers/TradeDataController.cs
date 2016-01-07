using Dashboard.Common;
using Dashboard.Logic;
using DashBoard.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DashBoard.Web.Areas.TradeData.Controllers
{
    public class TradeDataController : Controller
    {
        //
        // GET: /TradeData/TradeData/

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 策略交易明细视图控制器
        /// </summary>
        /// <returns></returns>
        public ActionResult StrategyDetail()
        {
            return View();
        }

        /// <summary>
        /// 用户自建策略视图控制器
        /// </summary>
        /// <returns></returns>
        public ActionResult CustomerCreateStrategy()
        {
            return View();
        }

        /// <summary>
        /// 策略交易明细数据获取控制器
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public ActionResult StrategyTradeDetail(jQueryDataTableParamModel param)
        {
            StrategyDetail da = new StrategyDetail();
            if (param.extra_search == null)
            {
                DateTime dt = DateTime.Now;
                da.BeginDate = dt.AddMonths(-1).ToString("yyyy-MM-dd");
                da.EndDate = dt.ToString("yyyy-MM-dd");
            }
            else
            {
                da.BeginDate = param.extra_search.Substring(0, 10);
                da.EndDate = param.extra_search.Substring(param.extra_search.IndexOf('-', 10) + 2, 10);
            }

            da.SearchColumns = param.searchColumns;

            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            string orderfield = null;
            if (sortColumnIndex == 1 || sortColumnIndex == 0)
            {
                orderfield = "strategyname";
            }
            if (sortColumnIndex == 2)
            {
                orderfield = "stratinfo";
            }
            if (sortColumnIndex == 3)
            {
                orderfield = "seriesno";
            }
            if (sortColumnIndex == 4)
            {
                orderfield = "groupname";
            }
            if (sortColumnIndex == 5)
            {
                orderfield = "createdate";
            }

            da.OrderField = orderfield;
            if(param.sSearch != null)
            {
                da.SearchColumns = param.sSearch;
            }
            else
            {
                da.SearchColumns = "";
            }
            var sortDirection = Request["sSortDir_0"]; // asc or desc
            da.SortDirection = sortDirection;
            da.DisplayLength = param.iDisplayLength;
            da.DisplayStart = param.iDisplayStart;
            da.CurrentPage = param.iDisplayStart / param.iDisplayLength;
            var result = DataServiceHelper.GetStrategyTradeDetail(da);


            var data = from c in result.List
                       select new[] { "",c.StrategyName, c.StratInfo,
                          c.SeriesNo, c.StrategyGroup,c.CreateDate };

            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = result.TotalRecords,
                iTotalDisplayRecords = result.TotalDisplayRecords,
                aaData = data
            },
                        JsonRequestBehavior.AllowGet);
        }
    }
}
