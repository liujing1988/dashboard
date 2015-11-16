using Dashboard.Common;
using Dashboard.Logic;
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

        public ActionResult AjaxHandler(jQueryDataTableParamModel param)
        {
            TradeDetails da = new TradeDetails();
            //for(int i =0;i<param.columnsearch.Count(); i++)
            
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


            var allCompanies = DataServiceHelper.GetTradeDetails(da);
            IEnumerable<TradeDetails> filteredCompanies;
            //if (!string.IsNullOrEmpty(param.sSearch))
            //{
            //    filteredCompanies = DataServiceHelper.GetTradeDetails(da)
            //             .Where(c => c.CustId.Contains(param.sSearch)
            //                         ||
            //              c.TradeDate.Contains(param.sSearch)
            //                         ||
            //                         c.StockCode.Contains(param.sSearch));
            //}
            //else 
            filteredCompanies = allCompanies;
            //    if (!string.IsNullOrEmpty(param.columnsearch))
            //{

            //    if (param.columnindex == "客户ID")
            //    {
            //        filteredCompanies = filteredCompanies
            //             .Where(c => c.CustId.Contains(param.columnsearch));
            //    }
            //    else if (param.columnindex == "交易日期")
            //    {
            //        filteredCompanies = filteredCompanies
            //             .Where(c => c.TradeDate.Contains(param.columnsearch));
            //    }
            //    else if (param.columnindex == "股票代码")
            //    {
            //        filteredCompanies = filteredCompanies
            //             .Where(c => c.StockCode.Contains(param.columnsearch));
            //    }
            //    else if (param.columnindex == "交易量")
            //    {
            //        filteredCompanies = filteredCompanies
            //             .Where(c => c.MatchQty.Contains(param.columnsearch));
            //    }
            //    else if (param.columnindex == "交易价格")
            //    {
            //        filteredCompanies = filteredCompanies
            //             .Where(c => c.MatchPrice.Contains(param.columnsearch));
            //    }
            //    else if (param.columnindex == "交易方向")
            //    {
            //        filteredCompanies = filteredCompanies
            //             .Where(c => c.BsFlag.Contains(param.columnsearch));
            //    }
            //    else
            //    {
            //        filteredCompanies = allCompanies;
            //    }
            //}
            //else
            //{
            //    filteredCompanies = allCompanies;
            //}

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
                          c.StockCode, c.MatchQty,c.MatchPrice,c.BsFlag };

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
