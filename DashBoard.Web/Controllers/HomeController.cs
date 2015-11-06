using Dashboard.Common;
using Dashboard.Logic;
using System;
using System.Collections.Generic;
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
        public ActionResult AjaxHandler(jQueryDataTableParamModel param)
        {
            TradeDetails da = new TradeDetails();
            if (param.extra_search == null)
            {
                DateTime dt = DateTime.Now;
                da.begindate = dt.AddMonths(-1).ToString("yyyy-MM-dd");
                da.enddate = dt.ToString("yyyy-MM-dd");
            }
            else
            {
                da.begindate = param.extra_search.Substring(0, 10);
                da.enddate = param.extra_search.Substring(param.extra_search.LastIndexOf('-', 22) + 2, 10);
            }


            var allCompanies = DemoService.GetTradeDetails(da);
            IEnumerable<TradeDetails> filteredCompanies;
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filteredCompanies = DemoService.GetTradeDetails(da)
                         .Where(c => c.CustId.Contains(param.sSearch)
                                     ||
                          c.TradeDate.Contains(param.sSearch)
                                     ||
                                     c.StockCode.Contains(param.sSearch));
            }
            else
            {
                filteredCompanies = allCompanies;
            }

            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);

            Func<TradeDetails, object> orderingFunction = null;
            if (sortColumnIndex == 0)
            {
                orderingFunction = (c => c.CustId);
            }
            if (sortColumnIndex == 1)
            {
                orderingFunction = (c => c.TradeDate);
            }
            if (sortColumnIndex == 2)
            {
                orderingFunction = (c => c.StockCode);
            }
            if (sortColumnIndex == 3)
            {
                orderingFunction = (c => c.MatchQty);
            }
            if (sortColumnIndex == 4)
            {
                orderingFunction = (c => c.MatchPrice);
            }
            if (sortColumnIndex == 5)
            {
                orderingFunction = (c => c.BsFlag);
            }

            var sortDirection = Request["sSortDir_0"]; // asc or desc
            if (sortDirection == "asc")
                filteredCompanies = filteredCompanies.OrderBy(orderingFunction);
            else
                filteredCompanies = filteredCompanies.OrderByDescending(orderingFunction);


            var displayedCompanies = filteredCompanies
                        .Skip(param.iDisplayStart)
                        .Take(param.iDisplayLength);

            var result = from c in displayedCompanies
                         select new[] { c.CustId, c.TradeDate,
                          c.StockCode, Convert.ToString(c.MatchQty),c.MatchPrice.ToString("f2"),c.BsFlag };

            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = allCompanies.Count(),
                iTotalDisplayRecords = filteredCompanies.Count(),
                aaData = result
            },
                        JsonRequestBehavior.AllowGet);
        }
    }
}
