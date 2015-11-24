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

            string ssql = "";
            if (!string.IsNullOrEmpty(param.searchColumns))
            {
                string[] columnIndexs = param.columnIndex.Split(',');
                string[] searchTexts = param.searchColumns.Split(',');
                for (int i = 0; i < 6; i++)
                {
                    if (param.columnIndex.Contains("客户ID"))
                    {
                        for (int j = 0; j < columnIndexs.Length; j++)
                        {
                            if (columnIndexs[j] == "客户ID")
                            {
                                ssql = searchTexts[j];
                            }
                        }
                    }
                    else
                    {
                        ssql = "";
                    }
                    if (param.columnIndex.Contains("交易日期"))
                    {
                        for (int j = 0; j < columnIndexs.Length; j++)
                        {
                            if (columnIndexs[j] == "交易日期")
                            {
                                ssql = ssql + ',' + searchTexts[j];
                            }
                        }
                    }
                    else
                    {
                        ssql = ssql + ',' + "";
                    }
                    if (param.columnIndex.Contains("股票代码"))
                    {
                        for (int j = 0; j < columnIndexs.Length; j++)
                        {
                            if (columnIndexs[j] == "股票代码")
                            {
                                ssql = ssql + ',' + searchTexts[j];
                            }
                        }
                    }
                    else
                    {
                        ssql = ssql + ',' + "";
                    }
                    if (param.columnIndex.Contains("交易量"))
                    {
                        for (int j = 0; j < columnIndexs.Length; j++)
                        {
                            if (columnIndexs[j] == "交易量")
                            {
                                ssql = ssql + ',' + searchTexts[j];
                            }
                        }
                    }
                    else
                    {
                        ssql = ssql + ',' + "";
                    }
                    if (param.columnIndex.Contains("交易价格"))
                    {
                        for (int j = 0; j < columnIndexs.Length; j++)
                        {
                            if (columnIndexs[j] == "交易价格")
                            {
                                ssql = ssql + ',' + searchTexts[j];
                            }
                        }
                    }
                    else
                    {
                        ssql = ssql + ',' + "";
                    }
                    if (param.columnIndex.Contains("交易方向"))
                    {
                        for (int j = 0; j < columnIndexs.Length; j++)
                        {
                            if (columnIndexs[j] == "交易方向")
                            {
                                ssql = ssql + ',' + searchTexts[j] + ',';
                            }
                        }
                    }
                    else
                    {
                        ssql = ssql + ',' + "" + ',';
                    }

                }
            }

            da.searchColumns = ssql;

            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            string orderfield = null;
            if (sortColumnIndex == 0)
            {
                orderfield = "custid";
            }
            if (sortColumnIndex == 1)
            {
                orderfield = "tradedate";
            }
            if (sortColumnIndex == 2)
            {
                orderfield = "stkcode";
            }
            if (sortColumnIndex == 3)
            {
                orderfield = "matchqty";
            }
            if (sortColumnIndex == 4)
            {
                orderfield = "matchprice";
            }
            if (sortColumnIndex == 5)
            {
                orderfield = "bsflag";
            }
            da.OrderField = orderfield;

            var sortDirection = Request["sSortDir_0"]; // asc or desc

            da.sortDirection = sortDirection;

            da.DisplayLength = param.iDisplayLength;

            da.DisplayStart = param.iDisplayStart;

            da.CurrentPage = param.iDisplayStart / param.iDisplayLength;

            var result = DataServiceHelper.GetTradeDetails(da);


            var data = from c in result.List
                       select new[] { c.CustId, c.TradeDate,
                          c.StockCode, c.MatchQty,c.MatchPrice,c.BsFlag };

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
