using Dashboard.Common;
using DashBoard.Common;
using DashBoard.Common.Data;
using DashBoard.Data;
using DashBoard.Logic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Logic
{
    /// <summary>
    /// 与数据库交互，为各类图表获取数据 修改日期：2015-11-24
    /// </summary>
    public class DataServiceHelper
    {
        /// <summary>
        /// 调用存储过程，获取新增客户和活跃客户信息
        /// </summary>
        /// <param name="da">查询条件，包括开始日期与结束日期</param>
        /// <returns>指定时间段内的新增用户、活跃用户和总用户数</returns>
        public static List<CustomerAmount> GetCustomer(CustomerAmount da)
        {
            List<CustomerAmount> result = new List<CustomerAmount>();
            int begindate = TranslateHelper.ConvertDate(da.BeginMonth);
            int enddate = TranslateHelper.ConvertDate(da.EndMonth);
            using (var db = new ModelDataContainer())
            {
                var list = db.sp_GetCustomer(begindate, enddate);
                foreach (var item in list)
                {
                    result.Add(new CustomerAmount()
                    {
                        CreatCustomer = item.createcustomer,
                        AliveCustomer = item.alivecustomer,
                        AllCustomerMonth = item.allmonth.ToString()
                    });
                }
            }
            return result;
        }

        /// <summary>
        /// 获取每月交易量
        /// </summary>
        /// <param name="dateTime">查询条件，包括开始月份与结束月份、策略名、策略类型、策略序列号</param>
        /// <returns>月份、该月所有用户交易量之和</returns>
        public static List<TradeMonthAmount> GetMatchAmount(StrategyDetail dateTime)
        {
            int bmonth = TranslateHelper.ConvertDate(dateTime.BeginDate);
            int emonth = TranslateHelper.ConvertDate(dateTime.EndDate);
            List<TradeMonthAmount> result = new List<TradeMonthAmount>();
            using (var db = new ModelDataContainer())
            {
                var query = from a in db.strategyorder
                            from c in db.strategyinfo
                            from d in db.strategykind
                            from e in db.menu
                            where a.strategyno == c.strategyno && c.kindid == d.kindid
                            where a.orderdate > 0 && (a.tradetype == "0" || a.tradetype == "1")
                            && c.menuid == e.id && e.name == "策略交易"
                            && (dateTime.StrategyName == null || c.strategyname.Contains(dateTime.StrategyName))
                            && (dateTime.StrategyKindName == null || d.kindname == dateTime.StrategyKindName)
                            && (dateTime.SeriesNo == null || c.seriesno == dateTime.SeriesNo)
                            where a.orderdate / 100 >= bmonth && a.orderdate / 100 <= emonth
                            && a.strategyno != -1 && a.cancelflag != "T" && a.orderstatus != "9" && a.orderstatus != "6"
                            group a by a.orderdate / 100 into b
                            select new
                            {
                                Month = b.Max(p => p.orderdate / 100),
                                MatchAmount = b.Sum(p => p.matchamt)
                            };
                foreach (var item in query.OrderBy(p => p.Month))
                {
                    result.Add(new TradeMonthAmount()
                    {
                        Month = item.Month.ToString(),
                        MatchAmount = item.MatchAmount,
                    });
                }
            }
            return result;
        }

        /// <summary>
        /// 获取指定策略开仓次数
        /// </summary>
        /// <param name="dateTime">查询条件，包括开始月份与结束月份、策略名、策略类型、策略序列号</param>
        /// <returns>符合条件的策略开仓次数之和</returns>
        public static List<OrderSend> GetOrderSendNum(StrategyDetail dateTime)
        {
            int bmonth = TranslateHelper.ConvertDate(dateTime.BeginDate);
            int emonth = TranslateHelper.ConvertDate(dateTime.EndDate);
            List<OrderSend> result = new List<OrderSend>();
            using (var db = new ModelDataContainer())
            {
                var query = (from d in
                                 (from a in db.positionbasicinfotable
                                  from c in db.strategyinfo
                                  from d in db.strategykind
                                  from h in db.menu
                                  where a.strategyno == c.strategyno && c.kindid == d.kindid
                                  where a.strategyno != -1 && a.createdate / 100 >= bmonth && a.createdate / 100 <= emonth
                                  && c.menuid == h.id && h.name == "策略交易"
                                  && (dateTime.StrategyName == null || c.strategyname.Contains(dateTime.StrategyName))
                                  && (dateTime.StrategyKindName == null || d.kindname == dateTime.StrategyKindName)
                                  && (dateTime.SeriesNo == null || c.seriesno == dateTime.SeriesNo)
                                  group a by new { a.custid, c.strategyname, a.createdate } into c
                                  select new
                                  {
                                      createdate = c.Key.createdate,
                                      strategyname = c.Key.strategyname,
                                      strategytradenum = c.Count()
                                  })
                             group d by new { month = d.createdate / 100 } into e
                             select new
                             {
                                 month = e.Key.month,
                                 strategytradenum = e.Count()
                             });
                foreach (var item in query.OrderBy(p => p.month))
                {
                    result.Add(new OrderSend()
                    {
                        Month = item.month.ToString(),
                        NumSend = item.strategytradenum
                    });
                }
            }
            return result;
        }


        /// <summary>
        /// 获取指定策略客户交易总量
        /// </summary>
        /// <param name="dateTime">查询条件，包括开始月份与结束月份、策略名、策略类型、策略序列号</param>
        /// <returns>符合条件的客户交易总量之和</returns>
        public static List<TopMatchQty> GetTopMatchAmt(StrategyDetail dateTime)
        {
            int bmonth = TranslateHelper.ConvertDate(dateTime.BeginDate);
            int emonth = TranslateHelper.ConvertDate(dateTime.EndDate);
            List<TopMatchQty> result = new List<TopMatchQty>();
            using (var db = new ModelDataContainer())
            {
                var list = db.sp_GetStrategyMatchAmt(bmonth, emonth, dateTime.StrategyName, dateTime.StrategyKindName, dateTime.SeriesNo).OrderBy(p=>p.tmatchamt);

                foreach (var item in list)
                {
                    if (item.tmatchamt != null)
                    {
                        result.Add(new TopMatchQty()
                        {

                            CustId = item.custid.ToString(),
                            StrategyName = item.strategyno,
                            MatchAmt = (decimal)(item.tmatchamt / 10000)

                        });
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 获取每日交易量（用于每月交易图表中的自动弹出日交易信息）
        /// </summary>
        /// <param name="dateTime">查询日期</param>
        /// <returns>日期、该日期所有用户交易量之和</returns>
        public static List<TradeMonthAmount> GetDateAmount(StrategyDetail dateTime)
        {
            int bday = TranslateHelper.ConvertDate(dateTime.BeginDate);
            List<TradeMonthAmount> result = new List<TradeMonthAmount>();
            using (var db = new ModelDataContainer())
            {
                var query = from a in db.strategyorder
                            from c in db.strategyinfo
                            from d in db.strategykind
                            from e in db.menu
                            where a.strategyno == c.strategyno && c.kindid == d.kindid
                            && (a.tradetype == "0" || a.tradetype == "1")
                            && a.strategyno != -1 && a.cancelflag != "T" && a.orderstatus != "9" && a.orderstatus != "6"
                            && c.menuid == e.id && e.name == "策略交易"
                            && (dateTime.StrategyName == null || c.strategyname.Contains(dateTime.StrategyName))
                            && (dateTime.StratInfo == null || d.kindname == dateTime.StratInfo)
                            && (dateTime.SeriesNo == null || c.seriesno == dateTime.SeriesNo)
                            where a.orderdate / 100 == bday
                            group a by a.orderdate into b
                            select new
                            {
                                Date = b.Max(p => p.orderdate),
                                DateAmount = b.Sum(p => p.matchamt)
                            };
                foreach (var item in query.OrderBy(p => p.Date))
                {
                    result.Add(new TradeMonthAmount()
                    {
                        Date = item.Date.ToString(),
                        DateAmount = item.DateAmount,
                    });
                }
            }
            return result;
        }

        /// <summary>
        /// 获取每分钟交易量（用于首页中的交易量柱状图，1分钟调用1次）
        /// </summary>
        /// <returns>时间、该时间对应的所有用户交易量之和</returns>
        public static List<RealTimeData> GetRealTimeData()
        {
            int tdate = Int32.Parse(DateTime.Now.ToString("yyyyMMdd"));
            List<RealTimeData> result = new List<RealTimeData>();
            using (var db = new ModelDataContainer())
            {
                var list = db.sp_TradeAmt_Minute(tdate);
                foreach (var item in list)
                {
                    if (item.rminute > 0)
                    {
                        result.Add(new RealTimeData()
                        {
                            Minute = TranslateHelper.ConvertTime(item.rminute),
                            TradeAmount = item.tradeamt,
                            Day = TranslateHelper.ConvertMinute(tdate)
                        });
                    }
                    else
                    {
                        result.Add(new RealTimeData()
                        {
                            Minute = item.rminute.ToString(),
                            TradeAmount = item.tradeamt,
                            Day = TranslateHelper.ConvertMinute(tdate)
                        });
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 用于调取综合查询中成交量柱状图所需数据
        /// </summary>
        /// <param name="da">日期参数，调用的起止日期</param>
        /// <returns>分钟值、对应交易量</returns>
        public static List<RealTimeData> GetRealData(RealTimeData da)
        {
            List<RealTimeData> result = new List<RealTimeData>();
            DateTime begindate = Convert.ToDateTime(da.BeginDate);
            DateTime enddate = Convert.ToDateTime(da.EndDate);
            for (DateTime i = begindate; i <= enddate; i = i.AddDays(1))
            {
                if (i.DayOfWeek.ToString() == "Saturday" || i.DayOfWeek.ToString() == "Sunday")
                {

                }
                else
                {
                    int indexdate = TranslateHelper.ConvertDate(i.ToString("yyyy-MM-dd"));
                    using (var db = new ModelDataContainer())
                    {
                        var list = db.sp_TradeAmt_Minute(indexdate);
                        foreach (var item in list)
                        {
                            if (item.rminute > 0)
                            {
                                result.Add(new RealTimeData()
                                {
                                    Minute = TranslateHelper.ConvertTime(item.rminute),
                                    TradeAmount = item.tradeamt,
                                    Day = i.ToString("yyyy-MM-dd")
                                });
                            }
                            else
                            {
                                result.Add(new RealTimeData()
                                {
                                    Minute = item.rminute.ToString(),
                                    TradeAmount = item.tradeamt,
                                    Day = i.ToString("yyyy-MM-dd")
                                });
                            }
                        }
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 调取系统功能模块交易金额
        /// </summary>
        /// <param name="dateTime">日期参数，查询的起止时间</param>
        /// <returns>功能模块名称、交易量</returns>
        public static List<Modules> GetStrategyType(GetDateTime dateTime)
        {
            int bdate = TranslateHelper.ConvertDate(dateTime.BeginDate);
            int edate = TranslateHelper.ConvertDate(dateTime.EndDate);
            List<Modules> result = new List<Modules>();
            using (var db = new ModelDataContainer())
            {
                var query = (from a in db.strategyorder
                             from b in db.strategyinfo
                             from c in db.menu
                             where (a.strategyno == b.strategyno) && (a.tradetype == "0" || a.tradetype == "1") && a.orderstatus != "9" && b.menuid == c.id
                             where a.cancelflag != "T" && a.orderstatus != "6" && c.name == "策略交易"
                             where (a.orderdate >= bdate && a.orderdate <= edate)
                             group a by new { b.strategyname } into h
                             select new
                             {
                                 strategytype = h.Key.strategyname,
                                 Nstrage = h.Sum(p => p.matchamt)
                             }
                    ).OrderByDescending(p=>p.Nstrage).Take(5);

                var total =
                    (from a in db.strategyorder
                     from b in db.strategyinfo
                     from c in db.menu
                     where (a.strategyno == b.strategyno) && (a.tradetype == "0" || a.tradetype == "1") && a.orderstatus != "9" && b.menuid == c.id
                     where a.cancelflag != "T" && a.orderstatus != "6" && c.name == "策略交易"
                     where (a.orderdate >= bdate && a.orderdate <= edate)
                     group a by new { c.name } into h
                     select new
                     {
                         strategytype = h.Key.name,
                         Nstrage = h.Sum(p => p.matchamt)
                     }
                    );
                decimal totalnum = 0;
                foreach(var item in total)
                {
                    if(item.Nstrage >= 0)
                    {
                        totalnum = (decimal)item.Nstrage;
                    }
                }

                decimal top = 0;
                foreach (var item in query)
                {
                    if (item.strategytype != " ")
                    {
                        result.Add(new Modules()
                        {
                            ModuleName = item.strategytype,
                            ModuleTradeAmt = item.Nstrage,
                        });
                        top += (decimal)item.Nstrage;
                    }
                }
                if (totalnum - top > 0)
                { 
                result.Add(new Modules()
                    {
                        ModuleName = "其他",
                        ModuleTradeAmt = totalnum - top
                    });
                }
            }
            return result;
        }

        /// <summary>
        /// 调取系统功能模块开仓次数
        /// </summary>
        /// <param name="dateTime">日期参数，查询的起止时间</param>
        /// <returns>功能模块名称、使用次数</returns>
        public static List<Modules> GetStrategyOpen(GetDateTime dateTime)
        {
            int bdate = TranslateHelper.ConvertDate(dateTime.BeginDate);
            int edate = TranslateHelper.ConvertDate(dateTime.EndDate);
            List<Modules> result = new List<Modules>();
            using (var db = new ModelDataContainer())
            {
                var query = (from d in
                                 (from a in db.positionbasicinfotable
                                  from b in db.strategyinfo
                                  from h in db.menu
                                  where a.strategyno != -1 && b.menuid == h.id && h.name == "策略交易"
                                  where a.strategyno == b.strategyno && a.createdate >= bdate && a.createdate <= edate
                                  group a by new { a.custid, b.strategyname, a.createdate } into c
                                  select new
                                  {
                                      createdate = c.Key.createdate,
                                      strategyname = c.Key.strategyname,
                                      strategytradenum = c.Count()
                                  })
                             group d by new { d.strategyname } into e
                             select new
                             {
                                 strategename = e.Key.strategyname,
                                 strategytradenum = e.Count()
                             }).OrderByDescending(p=>p.strategytradenum).Take(5);

                var total = (from d in
                                 (from a in db.positionbasicinfotable
                                  from b in db.strategyinfo
                                  from h in db.menu
                                  where a.strategyno != -1 && b.menuid == h.id && h.name == "策略交易"
                                  where a.strategyno == b.strategyno && a.createdate >= bdate && a.createdate <= edate
                                  group a by new { a.custid, h.name, a.createdate } into c
                                  select new
                                  {
                                      createdate = c.Key.createdate,
                                      modulename = c.Key.name,
                                      strategytradenum = c.Count()
                                  })
                             group d by new { d.modulename } into e
                             select new
                             {
                                 modulename = e.Key.modulename,
                                 strategytradenum = e.Count()
                             });

                var totalnum = 0;
                foreach(var item in total)
                {
                    if(item.strategytradenum >= 0)
                    {
                        totalnum = item.strategytradenum;
                    }
                }

                var top = 0;
                foreach (var item in query)
                {
                    if (item.strategename != " ")
                    {
                        result.Add(new Modules()
                        {
                            ModuleName = item.strategename,
                            NumModules = item.strategytradenum
                        });
                        top += item.strategytradenum;
                    }
                }
                if (totalnum - top > 0)
                {
                    result.Add(new Modules()
                    {
                        ModuleName = "其他",
                        NumModules = totalnum - top
                    });
                }
            }
            return result;
        }

        /// <summary>
        /// 获取用户当日交易总金额前五（用于首页展示）
        /// </summary>
        /// <returns>当日用户交易总量前五名的总交易金额、信用交易金额</returns>
        public static List<TradeDayAmount> GetDayAmount()
        {
            int tdate = Int32.Parse(DateTime.Now.ToString("yyyyMMdd"));
            List<TradeDayAmount> result = new List<TradeDayAmount>();
            using (var db = new ModelDataContainer())
            {

                //根据委托表进行统计 按custid的总交易量排名
                var creditTrade = from a in db.strategyorder
                                  where a.orderdate == tdate
                                  && a.strategyno != -1 && a.matchamt > 0 && a.tradetype == "1"
                                  && a.cancelflag != "T" && a.orderstatus != "9" && a.orderstatus != "6"
                                  group a by new {a.custid} into b
                                  select new
                                  {
                                      custid = b.Key.custid,
                                      credittrade = b.Sum(p=>p.matchamt)
                                  };
                var TotalTrade = (from a in db.strategyorder
                                  where a.orderdate == tdate
                                  && a.strategyno != -1 && a.matchamt > 0
                                  && a.cancelflag != "T" && a.orderstatus != "9" && a.orderstatus != "6"
                                  group a by new { a.custid } into b
                                  select new
                                  {
                                      custid = b.Key.custid,
                                      tradeamount = b.Sum(p => p.matchamt)
                                  }).OrderByDescending(p => p.tradeamount).Take(5);


                var query = (from a in TotalTrade
                            join b in creditTrade
                            on a.custid equals b.custid into trade
                            from c in trade.DefaultIfEmpty()
                            select new
                            {
                                custid = a.custid,
                                tradeamount = a.tradeamount,
                                credittrade = (c == null) ? 0 : c.credittrade
                            }).OrderByDescending(p=>p.tradeamount);

                foreach (var item in query)
                    result.Add(new TradeDayAmount()
                    {
                        CustId = item.custid,
                        TradeAmount = item.tradeamount,
                        CreditTrade = item.credittrade,
                        TradeDate = DateTime.Now.ToString("yyyy-MM-dd")

                    });
                
            }
            return result;
        }

        /// <summary>
        /// 获取融券卖出交易标的前十
        /// </summary>
        /// <returns>当日用户融券卖出总量前十名的总交易量、股票名称</returns>
        public static List<CreditTrade> GetCreditSalesAmount(RealTimeData da)
        {
            int tdate = Int32.Parse(DateTime.Now.ToString("yyyyMMdd"));
            int mdate = Int32.Parse(DateTime.Now.AddMonths(-1).ToString("yyyyMMdd"));
            int fdate = Int32.Parse(DateTime.Now.AddDays(-5).ToString("yyyyMMdd"));
            int begindate = tdate;
            int enddate = tdate;
            if (da.Day == "30")
            {
                begindate = mdate;
            }
            if (da.Day == "5")
            {
                begindate = fdate;
            }
            List<CreditTrade> result = new List<CreditTrade>();
            using (var db = new ModelDataContainer())
            {
                var query = (from a in db.strategyorder
                             where a.orderdate >= begindate && a.orderdate <= enddate
                             && a.matchqty > 0 && a.bsflag == "S"
                             && a.cancelflag != "T" && a.orderstatus != "9" && a.orderstatus != "6"
                             group a by new { a.stkcode,a.market } into b
                             select new
                             {
                                 stockcode = b.Key.stkcode,
                                 market = b.Key.market,
                                 tradeamount = b.Sum(p => p.matchqty)
                             }).OrderByDescending(p => p.tradeamount).Take(10);
                StockDicManager.LoadDict();
                foreach (var item in query)
                    result.Add(new CreditTrade()
                    {
                        StockName = StockDicManager.GetStockName(item.stockcode,item.market),
                        MatchQty = item.tradeamount.ToString(),
                        TradeDate = DateTime.Now.ToString("yyyy-MM-dd")
                    });
            }
            return result;
        }

        /// <summary>
        /// 获取融资买入交易标的前十
        /// </summary>
        /// <returns>当日用户融资买入总量前十名的总交易量、股票名称</returns>
        //public static List<CreditTrade> GetCreditBuyAmount(RealTimeData da)
        //{
        //    int tdate = Int32.Parse(DateTime.Now.ToString("yyyyMMdd"));
        //    int mdate = Int32.Parse(DateTime.Now.AddMonths(-1).ToString("yyyyMMdd"));
        //    int fdate = Int32.Parse(DateTime.Now.AddDays(-5).ToString("yyyyMMdd"));
        //    int begindate = tdate;
        //    int enddate = tdate;
        //    if (da.Day == "30")
        //    {
        //        begindate = mdate;
        //    }
        //    if (da.Day == "5")
        //    {
        //        begindate = fdate;
        //    }

        //    List<CreditTrade> result = new List<CreditTrade>();
        //    using (var db = new ModelDataContainer())
        //    {
        //        var bamt = from a in db.strategyorder
        //                   where a.tradetype == "1"
        //                   where a.orderdate >= begindate && a.orderdate <= enddate
        //                   && a.cancelflag != "T" && a.orderstatus != "9" && a.orderstatus != "6"
        //                   && a.matchqty > 0 && a.bsflag == "B"
        //                   group a by new { a.stkcode,a.market } into b
        //                   select new
        //                   {
        //                       info = b.Key.stkcode + b.Key.market,
        //                       stockcode = b.Key.stkcode,
        //                       market = b.Key.market,
        //                       btradeamount = b.Sum(p => p.matchqty)
        //                   };
        //        var samt = from a in db.strategyorder
        //                   where a.tradetype == "1"
        //                   where a.orderdate >= begindate && a.orderdate <= enddate
        //                   && a.cancelflag != "T" && a.orderstatus != "9" && a.orderstatus != "6"
        //                   && a.matchqty > 0 && a.bsflag == "S"
        //                   group a by new { a.stkcode,a.market } into b
        //                   select new
        //                   {
        //                       info = b.Key.stkcode + b.Key.market,
        //                       stockcode = b.Key.stkcode,
        //                       market = b.Key.market,
        //                       stradeamount = b.Sum(p => p.matchqty)
        //                   };
        //        var b_s = from a in bamt
        //                  join b in samt
        //                  on a.info equals b.info into BS
        //                  from c in BS.DefaultIfEmpty()
        //                  select new
        //                  {
        //                      stockcode = a.stockcode,
        //                      market = a.market,
        //                      btradeamount = a.btradeamount,
        //                      stradeamount = (c == null) ? 0 : c.stradeamount
        //                  };

        //        var s_b = from a in samt
        //                  join b in bamt
        //                  on a.info equals b.info into SB
        //                  from c in SB.DefaultIfEmpty()
        //                  select new
        //                  {
        //                      stockcode = a.stockcode,
        //                      market = a.market,
        //                      btradeamount = (c == null) ? 0 : c.btradeamount,
        //                      stradeamount = a.stradeamount
        //                  };
        //        var b_samt = b_s.Concat(s_b).Distinct();

        //        var query = b_samt.OrderByDescending(p => (p.btradeamount - p.stradeamount)).Take(10);
        //        StockDicManager.LoadDict();
        //        foreach (var item in query)
        //            result.Add(new CreditTrade()
        //            {
        //                StockName = StockDicManager.GetStockName(item.stockcode,item.market),
        //                MatchQty = (item.btradeamount - item.stradeamount).ToString(),
        //                TradeDate = DateTime.Now.ToString("yyyy-MM-dd")
        //            });
        //    }
        //    return result;
        //}

        /// <summary>
        /// 调用存储过程，获取客户交易流水
        /// </summary>
        /// <param name="da.begindate">开始时间</param>
        /// <param name="da.enddate">结束时间</param>
        /// <param name="da.searchColumns">搜索内容</param>
        /// <param name="da.DisplayStart">当前页第一行数据的总行号</param>
        /// <param name="da.DisplayLength">每页显示数据行数</param>
        /// <param name="da.CurrentPage">当前页号</param>
        /// <param name="da.sortDirection">排序方式（正序、反序）</param>
        /// <param name="da.OrderField">排序字段</param>
        /// <returns>符合条件的数据集、符合条件的记录条数、总页数</returns>
        public static RecordResult<TradeDetails> GetTradeDetails(TradeDetails da)
        {
            RecordResult<TradeDetails> result = new RecordResult<TradeDetails>();
            var db = DbFactory.Create();

            List<DbParameter> dbParameters = new List<DbParameter>();
            int begindate = TranslateHelper.ConvertDate(da.begindate);
            int enddate = TranslateHelper.ConvertDate(da.enddate);

            dbParameters.Add(db.NewParameter("BeginDate", begindate, DbType.Int32));
            dbParameters.Add(db.NewParameter("EndDate", enddate, DbType.Int32));
            dbParameters.Add(db.NewParameter("SearchColumns", da.searchColumns, DbType.String));
            dbParameters.Add(db.NewParameter("DisplayStart", da.DisplayStart, DbType.Int32));
            dbParameters.Add(db.NewParameter("DisplayLength", da.DisplayLength, DbType.Int32));
            dbParameters.Add(db.NewParameter("CurrentPage", da.CurrentPage, DbType.Int32));
            dbParameters.Add(db.NewParameter("SortDirection", da.sortDirection, DbType.String));
            dbParameters.Add(db.NewParameter("OrderField", da.OrderField, DbType.String));

            var pageCount = db.NewParameter("PageCount", 0, DbType.Int32);
            pageCount.Direction = ParameterDirection.Output;
            var totalRecords = db.NewParameter("TotalRecords", 0, DbType.Int32);
            totalRecords.Direction = ParameterDirection.Output;
            var totalDisplayRecords = db.NewParameter("TotalDisplayRecords", 0, DbType.Int32);
            totalDisplayRecords.Direction = ParameterDirection.Output;
            dbParameters.Add(pageCount);
            dbParameters.Add(totalRecords);
            dbParameters.Add(totalDisplayRecords);
            DataTable dt = db.GetDataTable("sp_GetCustomerTradeDetail", dbParameters);

            result.List = new List<TradeDetails>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRowReader reader = new DataRowReader(dt.Rows[i]);
                TradeDetails detail = new TradeDetails()
                {
                    CustId = reader.GetString("custid"),
                    TradeDate = reader.GetString("tradedate"),
                    StockCode = reader.GetString("stkcode"),
                    MatchQty = reader.GetString("matchqty"),
                    MatchPrice = Math.Round(reader.GetDouble("matchprice"),4).ToString("0.0000"),
                    BsFlag = TranslateHelper.ConvertBSFlag(reader.GetString("bsflag"))
                };
                result.List.Add(detail);
            }
            result.TotalPages = DataConvert.ToInt32(pageCount.Value);
            result.TotalRecords = DataConvert.ToInt32(totalRecords.Value);
            result.TotalDisplayRecords = DataConvert.ToInt32(totalDisplayRecords.Value);
            return result;
        }

        /// <summary>
        /// 调用存储过程，获取撤单客户的交易概览
        /// </summary>
        /// <param name="da.begindate">开始时间</param>
        /// <param name="da.enddate">结束时间</param>
        /// <param name="da.searchColumns">搜索内容</param>
        /// <param name="da.DisplayStart">当前页第一行数据的总行号</param>
        /// <param name="da.DisplayLength">每页显示数据行数</param>
        /// <param name="da.CurrentPage">当前页号</param>
        /// <param name="da.sortDirection">排序方式（正序、反序）</param>
        /// <param name="da.OrderField">排序字段</param>
        /// <returns>符合条件的数据集、符合条件的记录条数、总页数</returns>
        public static RecordResult<TradeDetails> GetRevokeMain(TradeDetails da)
        {
            RecordResult<TradeDetails> result = new RecordResult<TradeDetails>();
            var db = DbFactory.Create();

            List<DbParameter> dbParameters = new List<DbParameter>();
            int begindate = TranslateHelper.ConvertDate(da.begindate);
            int enddate = TranslateHelper.ConvertDate(da.enddate);

            dbParameters.Add(db.NewParameter("BeginDate", begindate, DbType.Int32));
            dbParameters.Add(db.NewParameter("EndDate", enddate, DbType.Int32));
            dbParameters.Add(db.NewParameter("SearchColumns", da.searchColumns, DbType.String));
            dbParameters.Add(db.NewParameter("DisplayStart", da.DisplayStart, DbType.Int32));
            dbParameters.Add(db.NewParameter("DisplayLength", da.DisplayLength, DbType.Int32));
            dbParameters.Add(db.NewParameter("CurrentPage", da.CurrentPage, DbType.Int32));
            dbParameters.Add(db.NewParameter("SortDirection", da.sortDirection, DbType.String));
            dbParameters.Add(db.NewParameter("OrderField", da.OrderField, DbType.String));
            dbParameters.Add(db.NewParameter("LimitNumOrder", da.LimitMinOrder, DbType.Int32));
            var pageCount = db.NewParameter("PageCount", 0, DbType.Int32);
            pageCount.Direction = ParameterDirection.Output;
            var totalRecords = db.NewParameter("TotalRecords", 0, DbType.Int32);
            totalRecords.Direction = ParameterDirection.Output;
            var totalDisplayRecords = db.NewParameter("TotalDisplayRecords", 0, DbType.Int32);
            totalDisplayRecords.Direction = ParameterDirection.Output;
            dbParameters.Add(pageCount);
            dbParameters.Add(totalRecords);
            dbParameters.Add(totalDisplayRecords);
            DataTable dt = db.GetDataTable("[sp_GetRevokePercent]", dbParameters);

            result.List = new List<TradeDetails>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRowReader reader = new DataRowReader(dt.Rows[i]);
                TradeDetails detail = new TradeDetails()
                {
                    CustId = reader.GetString("custid"),
                    OrderDate = reader.GetString("orderdate"),
                    PercentRevoke = reader.GetDecimal("percentrevoke").ToString("f2"),
                    NumRevoke = reader.GetString("numrevoke"),
                    NumOrder = reader.GetString("numorder"),
                    MiNumOrder = reader.GetString("maxminuteorder"),
                    SeNumOrder = reader.GetString("maxsecondorder"),
                    NetBuyAmt = reader.GetDecimal("netbuyamt").ToString("f2")
                };
                result.List.Add(detail);
            }
            result.TotalPages = DataConvert.ToInt32(pageCount.Value);
            result.TotalRecords = DataConvert.ToInt32(totalRecords.Value);
            result.TotalDisplayRecords = DataConvert.ToInt32(totalDisplayRecords.Value);
            return result;
        }

        /// <summary>
        /// 调用存储过程，获取撤单客户的交易流水
        /// </summary>
        /// <param name="da.begindate">开始时间</param>
        /// <param name="da.enddate">结束时间</param>
        /// <param name="da.searchColumns">搜索内容</param>
        /// <param name="da.DisplayStart">当前页第一行数据的总行号</param>
        /// <param name="da.DisplayLength">每页显示数据行数</param>
        /// <param name="da.CurrentPage">当前页号</param>
        /// <param name="da.sortDirection">排序方式（正序、反序）</param>
        /// <param name="da.OrderField">排序字段</param>
        /// <returns>符合条件的数据集、符合条件的记录条数、总页数</returns>
        public static RecordResult<TradeDetails> GetRevokeDetails(TradeDetails da)
        {
            RecordResult<TradeDetails> result = new RecordResult<TradeDetails>();
            var db = DbFactory.Create();

            List<DbParameter> dbParameters = new List<DbParameter>();
            int begindate = TranslateHelper.ConvertDate(da.begindate);
            int enddate = TranslateHelper.ConvertDate(da.enddate);

            dbParameters.Add(db.NewParameter("BeginDate", begindate, DbType.Int32));
            dbParameters.Add(db.NewParameter("EndDate", enddate, DbType.Int32));
            dbParameters.Add(db.NewParameter("SearchColumns", da.searchColumns, DbType.String));
            dbParameters.Add(db.NewParameter("DisplayStart", da.DisplayStart, DbType.Int32));
            dbParameters.Add(db.NewParameter("DisplayLength", da.DisplayLength, DbType.Int32));
            dbParameters.Add(db.NewParameter("CurrentPage", da.CurrentPage, DbType.Int32));
            dbParameters.Add(db.NewParameter("SortDirection", da.sortDirection, DbType.String));
            dbParameters.Add(db.NewParameter("OrderField", da.OrderField, DbType.String));

            var pageCount = db.NewParameter("PageCount", 0, DbType.Int32);
            pageCount.Direction = ParameterDirection.Output;
            var totalRecords = db.NewParameter("TotalRecords", 0, DbType.Int32);
            totalRecords.Direction = ParameterDirection.Output;
            var totalDisplayRecords = db.NewParameter("TotalDisplayRecords", 0, DbType.Int32);
            totalDisplayRecords.Direction = ParameterDirection.Output;
            dbParameters.Add(pageCount);
            dbParameters.Add(totalRecords);
            dbParameters.Add(totalDisplayRecords);
            DataTable dt = db.GetDataTable("sp_RevokeTradeDetail", dbParameters);

            result.List = new List<TradeDetails>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRowReader reader = new DataRowReader(dt.Rows[i]);
                TradeDetails detail = new TradeDetails()
                {
                    CustId = reader.GetString("custid"),
                    OrderDate = reader.GetString("orderdate"),
                    OperTime = reader.GetString("opertime"),
                    StockCode = TranslateHelper.ConvertMarket(reader.GetString("market")) + " " + reader.GetString("stkcode"),
                    OrderQty = reader.GetString("orderqty"),
                    BsFlag = TranslateHelper.ConvertBSFlag(reader.GetString("bsflag")),
                    CancelFlag = TranslateHelper.ConvertCancelFlag(reader.GetString("cancelflag"))
                };
                result.List.Add(detail);
            }
            result.TotalPages = DataConvert.ToInt32(pageCount.Value);
            result.TotalRecords = DataConvert.ToInt32(totalRecords.Value);
            result.TotalDisplayRecords = DataConvert.ToInt32(totalDisplayRecords.Value);
            return result;
        }

        /// <summary>
        /// 获取本月策略开仓金额前五
        /// </summary>
        /// <returns>策略名称、开仓金额</returns>
        public static List<Modules> GetStrategyTradeAmt()
        {
            List<Modules> result = new List<Modules>();
            int tdate = Int32.Parse(DateTime.Now.ToString("yyyyMMdd"));
            using (var db = new ModelDataContainer())
            {
                var query = (from a in db.strategyorder
                             from b in db.strategyinfo
                             from d in db.menu
                             where a.strategyno != -1 && a.cancelflag != "T" && a.orderstatus != "9" && a.orderstatus != "6" && b.menuid == d.id
                             where a.strategyno == b.strategyno && a.orderdate / 100 == tdate / 100 && d.name == "策略交易"
                             group a by new { b.strategyname } into c
                             select new
                             {
                                 strategyname = c.Key.strategyname,
                                 strategytradeamt = c.Sum(p => p.matchamt)
                             }).OrderByDescending(p => p.strategytradeamt).Take(5);

                foreach (var item in query)
                {
                    result.Add(new Modules()
                    {
                        ModuleName = item.strategyname,
                        ModuleTradeAmt = item.strategytradeamt
                    });
                }
            }
            return result;
        }

        /// <summary>
        /// 获取策略交易明细
        /// </summary>
        /// <param name="parameters.BeginDate">开始时间</param>
        /// <param name="parameters.EndDate">结束时间</param>
        /// <param name="parameters.SearchColumns">搜索内容</param>
        /// <param name="parameters.DisplayStart">当前页第一行数据的总行号</param>
        /// <param name="parameters.DisplayLength">每页显示数据行数</param>
        /// <param name="parameters.CurrentPage">当前页号</param>
        /// <param name="parameters.SortDirection">排序方式（正序、反序）</param>
        /// <param name="parameters.OrderField">排序字段</param>
        /// <returns>策略名、策略描述、序列号、策略组、开仓时间</returns>
        public static RecordResult<StrategyDetail> GetStrategyTradeDetail(StrategyDetail parameters)
        {
            RecordResult<StrategyDetail> result = new RecordResult<StrategyDetail>();
            List<DbParameter> dbParameters = new List<DbParameter>();
            var db = DbFactory.Create();
            int begindate = TranslateHelper.ConvertDate(parameters.BeginDate);
            int enddate = TranslateHelper.ConvertDate(parameters.EndDate);
            dbParameters.Add(db.NewParameter("BeginDate", begindate, DbType.Int32));
            dbParameters.Add(db.NewParameter("EndDate", enddate, DbType.Int32));
            dbParameters.Add(db.NewParameter("SearchColumns", parameters.SearchColumns, DbType.String));
            dbParameters.Add(db.NewParameter("DisplayStart", parameters.DisplayStart, DbType.Int32));
            dbParameters.Add(db.NewParameter("DisplayLength", parameters.DisplayLength, DbType.Int32));
            dbParameters.Add(db.NewParameter("CurrentPage", parameters.CurrentPage, DbType.Int32));
            dbParameters.Add(db.NewParameter("SortDirection", parameters.SortDirection, DbType.String));
            dbParameters.Add(db.NewParameter("OrderField", parameters.OrderField, DbType.String));

            var pageCount = db.NewParameter("PageCount", 0, DbType.Int32);
            pageCount.Direction = ParameterDirection.Output;
            var totalRecords = db.NewParameter("TotalRecords", 0, DbType.Int32);
            totalRecords.Direction = ParameterDirection.Output;
            var totalDisplayRecords = db.NewParameter("TotalDisplayRecords", 0, DbType.Int32);
            totalDisplayRecords.Direction = ParameterDirection.Output;
            dbParameters.Add(pageCount);
            dbParameters.Add(totalRecords);
            dbParameters.Add(totalDisplayRecords);
            DataTable dt = db.GetDataTable("sp_GetStrategyTradeDetail", dbParameters);

            result.List = new List<StrategyDetail>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRowReader reader = new DataRowReader(dt.Rows[i]);
                StrategyDetail detail = new StrategyDetail()
                {
                    StrategyName = reader.GetString("strategyname"),
                    StratInfo = reader.GetString("stratinfo"),
                    SeriesNo = reader.GetString("seriesno"),
                    StrategyGroup = reader.GetString("groupname"),
                    CreateDate = reader.GetString("createdate")
                };
                result.List.Add(detail);
            }
            result.TotalPages = DataConvert.ToInt32(pageCount.Value);
            result.TotalRecords = DataConvert.ToInt32(totalRecords.Value);
            result.TotalDisplayRecords = DataConvert.ToInt32(totalDisplayRecords.Value);
            return result;
        }

        /// <summary>
        /// 获取策略交易明细子项
        /// </summary>
        /// <param name="parameters.BeginDate">开仓日期</param>
        /// <param name="parameters.StrategyName">策略名</param>
        /// <returns>用户ID，开仓时间、股票编号、股票名称、交易方向</returns>
        public static List<StrategyDetail> GetSubStrategyTradeDetail(StrategyDetail parameters)
        {
            List<StrategyDetail> result = new List<StrategyDetail>();
            List<DbParameter> dbParameters = new List<DbParameter>();
            var db = DbFactory.Create();
            int begindate = TranslateHelper.ConvertDate(parameters.BeginDate);
            dbParameters.Add(db.NewParameter("CreateDate", begindate, DbType.Int32));
            dbParameters.Add(db.NewParameter("StrategyName", parameters.StrategyName, DbType.String));
            DataTable dt = db.GetDataTable("sp_GetSubStrategyTradeDetail", dbParameters);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRowReader reader = new DataRowReader(dt.Rows[i]);
                StrategyDetail detail = new StrategyDetail()
                {
                    CustId = reader.GetString("custid"),
                    CreateTime = reader.GetString("createtime"),
                    StockCode = reader.GetString("stockcode"),
                    StockName = reader.GetString("stockname"),
                    BSFlag = reader.GetString("bsflag"),
                    OrderQty = reader.GetString("orderqty")
                };
                result.Add(detail);
            }
            return result;
        }

        /// <summary>
        /// 获取本月策略开仓次数前五
        /// </summary>
        /// <returns>策略名称、开仓次数</returns>
        public static List<Modules> GetStrategyTradeAct()
        {
            List<Modules> result = new List<Modules>();
            int tdate = Int32.Parse(DateTime.Now.ToString("yyyyMMdd"));
            using (var db = new ModelDataContainer())
            {
                var query = (from e in
                                 (from a in db.positionbasicinfotable
                                  from b in db.strategyinfo
                                  from c in db.menu
                                  where a.strategyno != -1
                                  where a.strategyno == b.strategyno && a.createdate / 100 == tdate / 100
                                  && b.menuid == c.id
                                  group a by new { a.custid, c.name, a.createdate } into d
                                  select new
                                  {
                                      createdate = d.Key.createdate,
                                      modulename = d.Key.name,
                                      strategytradenum = d.Count(),
                                      tradeaccount = d.Sum(p=>p.openamount) + d.Sum(p=>p.closeamount)
                                  })
                             group e by new { modulename = e.modulename} into f
                             select new
                             {
                                 modulename = f.Key.modulename,
                                 strategytradenum = f.Count(),
                                 tradeaccount = f.Sum(p=>p.tradeaccount)
                             }).OrderByDescending(p => p.strategytradenum).Take(5);

                foreach (var item in query)
                {
                    result.Add(new Modules()
                    {
                        ModuleName = item.modulename,
                        NumModules = item.strategytradenum,
                        ModuleTradeAmt = item.tradeaccount
                    });
                }
            }
            return result;
        }

        /// <summary>
        /// 获取当前在线用户数和信用用户数
        /// </summary>
        /// <returns>在线用户数、其中信用用户数</returns>
        public static CustomerOnline GetCustomerOnline()
        {
            CustomerOnline result = new CustomerOnline();
            int tdate = Int32.Parse(DateTime.Now.ToString("yyyyMMdd"));
            int ttime = Int32.Parse(DateTime.Now.ToString("HHmm"));
            using (var db = new ModelDataContainer())
            {
                var numcust = from a in db.strategyorder
                              where a.orderdate == tdate
                              && (a.tradetype == "0" || a.tradetype == "1") 
                              group a by a.custid into b
                              select new
                              {
                                  b.Key,
                                  numcust = b.Count()
                              };
                var creditcust = from a in db.strategyorder
                                 where a.orderdate == tdate
                                 && a.tradetype == "1"
                                 group a by a.custid into b
                                 select new
                                 {
                                     creditcust = b.Count()
                                 };

                result.NumCustomerOnline = numcust.Count();
                result.CreditCustomer = creditcust.Count();
                //foreach (var item in numcust)
                //{
                //    result.NumCustomerOnline = item.numcust;
                //}
                //foreach (var item in creditcust)
                //{
                //    result.CreditCustomer = item.creditcust;
                //}
                result.ServerDate = DateTime.Now.ToString("yyyy-MM-dd");
            }
            return result;
        }

        /// <summary>
        /// 获取当日委托、成交、撤单情况(每秒更新)
        /// </summary>
        /// <returns>当日委托笔数、成交笔数、撤单笔数</returns>
        public static TradeDayVolume GetTradeDayVolume()
        {
            TradeDayVolume result = new TradeDayVolume();
            int tdate = Int32.Parse(DateTime.Now.ToString("yyyyMMdd"));
            int ttime = Int32.Parse(DateTime.Now.ToString("HHmmss"));
            using (var db = new ModelDataContainer())
            {
                //当日委托笔数
                var ordercount = from a in db.strategyorder
                                 where a.orderdate == tdate && a.cancelflag == "F"
                                 && a.orderstatus != "9" && a.orderstatus != "6"
                                 group a by a.orderdate into b
                                 select new
                                 {
                                     ordercount = b.Count()
                                 };
                foreach (var item in ordercount)
                {
                    result.NumOrder = item.ordercount;
                }

                //日最大委托笔数（每分钟）
                var miordercount = from a in db.strategyorder
                                   where a.orderdate == tdate
                                   && a.cancelflag == "F"
                                   && a.orderstatus != "9" && a.orderstatus != "6"
                                   group a by a.opertime / 10000 into b
                                   select new
                                   {
                                       miordercount = b.Count()
                                   };
                if (miordercount.Count() > 0)
                {
                    result.MiNumOrder = miordercount.Max(p => p.miordercount);
                }
                else
                {
                    result.MiNumOrder = 0;
                }


                //最大委托笔数（每秒）
                var seordercount = from a in db.strategyorder
                                   where a.orderdate == tdate
                                   && a.cancelflag == "F"
                                   && a.orderstatus != "9" && a.orderstatus != "6"
                                   group a by (a.opertime / 100) into b
                                   select new
                                   {
                                       seordercount = b.Count()
                                   };
                if (seordercount.Count() > 0)
                {
                    result.SeNumOrder = seordercount.Max(p => p.seordercount);
                }
                else
                {
                    result.SeNumOrder = 0;
                }


                //当日交易量
                var volumecount = from a in db.strategyorder
                                  where a.orderdate == tdate && a.cancelflag == "F"
                                  && a.orderstatus != "9" && a.matchqty > 0 && a.orderstatus != "6"
                                  group a by a.orderdate into b
                                  select new
                                  {
                                      volumecount = b.Count()
                                  };
                foreach (var item in volumecount)
                {
                    result.NumVolum = item.volumecount;
                }

                //当日撤单笔数
                var cancelcount = from a in db.strategyorder
                                  where a.orderdate == tdate && a.cancelflag == "F"
                                  && (a.orderstatus == "5" || a.orderstatus == "6")
                                  group a by a.orderdate into b
                                  select new
                                  {
                                      cancelcount = b.Count()
                                  };
                foreach (var item in cancelcount)
                {
                    result.NumRevoke = item.cancelcount;
                }

                //当日成交金额
                var volumeamt = from a in db.strategyorder
                                where a.orderdate == tdate && a.cancelflag == "F"
                                && a.orderstatus != "9" && a.matchqty > 0 && a.orderstatus != "6"
                                group a by a.orderdate into b
                                select new
                                {
                                    volumeamt = b.Sum(p => p.matchamt)
                                };
                foreach (var item in volumeamt)
                {
                    result.VolumAmt = item.volumeamt.ToString();
                }

                //当日逆回购金额
                var rrpamt = from a in db.strategyorder
                             where a.orderdate == tdate && a.cancelflag == "F"
                             && a.orderstatus != "9" && a.matchqty > 0 && a.orderstatus != "6"
                             && a.bsflag == "["
                             group a by a.orderdate into b
                             select new
                             {
                                 rrpamt = b.Sum(p => p.matchamt)
                             };
                foreach (var item in rrpamt)
                {
                    result.RRPAmt = item.rrpamt.ToString();
                }

                //当日买入金额
                var buyamt = from a in db.strategyorder
                             where a.orderdate == tdate && a.cancelflag == "F"
                             && a.orderstatus != "9" && a.matchqty > 0 && a.orderstatus != "6"
                             && a.bsflag == "B"
                             group a by a.orderdate into b
                             select new
                             {
                                 buyamt = b.Sum(p => p.matchamt)
                             };
                foreach (var item in buyamt)
                {
                    result.BuyAmt = item.buyamt.ToString();
                }

                //当日卖出金额
                var salesamt = from a in db.strategyorder
                               where a.orderdate == tdate && a.cancelflag == "F"
                               && a.orderstatus != "9" && a.matchqty > 0 && a.orderstatus != "6"
                               && a.bsflag == "S"
                               group a by a.orderdate into b
                               select new
                               {
                                   salesamt = b.Sum(p => p.matchamt)
                               };
                foreach (var item in salesamt)
                {
                    result.SalesAmt = item.salesamt.ToString();
                }

            }
            return result;
        }

        /// <summary>
        /// 获取用户自建策略手动上载和自动上传策略的交易量情况
        /// </summary>
        /// <param name="da.BeginDate">开始月份</param>
        /// <param name="da.EndDate">结束月份</param>
        /// <returns>月份、自动上传交易量、手动上载交易量列表</returns>
        public static List<StrategyDetail> GetCustomerCreatedStrategy(StrategyDetail da)
        {
            List<StrategyDetail> result = new List<StrategyDetail>();
            List<DbParameter> parameter = new List<DbParameter>();
            var db = DbFactory.Create();
            int beginmonth = TranslateHelper.ConvertDate(da.BeginDate);
            int endmonth = TranslateHelper.ConvertDate(da.EndDate);
            parameter.Add(db.NewParameter("Beginmonth", beginmonth, DbType.Int32));
            parameter.Add(db.NewParameter("Endmonth", endmonth, DbType.Int32));

            DataTable dt = db.GetDataTable("sp_CustomerCreateStrategy", parameter);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRowReader reader = new DataRowReader(dt.Rows[i]);
                StrategyDetail detail = new StrategyDetail()
                {
                    Month = reader.GetInt32("month"),
                    AutoAmt = reader.GetDecimal("autoamt"),
                    ManualAmt = reader.GetDecimal("manualamt")
                };
                result.Add(detail);
            }
            return result;
        }

        /// <summary>
        /// 获取用户自建策略客户交易量TOP5
        /// </summary>
        /// <param name="da.BeginDate">开始月份</param>
        /// <param name="da.EndDate">开始月份</param>
        /// <returns>符合条件的客户交易总量之和</returns>
        public static List<TopMatchQty> GetCustomerCreateStrategyTopMatchQty(StrategyDetail da)
        {
            int bmonth = TranslateHelper.ConvertDate(da.BeginDate);
            int emonth = TranslateHelper.ConvertDate(da.EndDate);
            List<TopMatchQty> result = new List<TopMatchQty>();
            using (var db = new ModelDataContainer())
            {
                var query = (from a in db.strategyorder
                             from b in db.strategyinfo
                             from c in db.generalparam
                             where a.strategyno == b.strategyno && b.strategyno == c.strategyno
                             && b.isprivate == "1" && a.strategyno != -1
                             && (a.tradetype == "0" || a.tradetype == "1") && a.orderstatus != "6" && a.cancelflag != "T"
                             && a.orderstatus != "9"
                             && a.orderdate / 100 >= bmonth && a.orderdate / 100 <= emonth
                             group a by new { a.custid } into d
                             select new
                             {
                                 custid = d.Key.custid,
                                 matchamt = d.Sum(p => p.matchamt)
                             }).OrderByDescending(p => p.matchamt).Take(5);
                foreach (var item in query)
                {
                    result.Add(new TopMatchQty()
                    {

                        CustId = item.custid.ToString(),
                        MatchAmt = (decimal)item.matchamt

                    });
                }
            }
            return result;
        }

        /// <summary>
        /// 获取新增用户情况
        /// </summary>
        /// <param name="da.BeginMonth">开始月份</param>
        /// <param name="da.EndMonth">结束月份</param>
        /// <returns>月份、新增机构用户数、新增个人用户数</returns>
        public static List<CustomerAmount> GetCreateCustomer(CustomerAmount da)
        {
            List<CustomerAmount> result = new List<CustomerAmount>();
            List<DbParameter> parameter = new List<DbParameter>();
            var db = DbFactory.Create();
            int beginmonth = TranslateHelper.ConvertDate(da.BeginMonth);
            int endmonth = TranslateHelper.ConvertDate(da.EndMonth); ;
            parameter.Add(db.NewParameter("Begindate", beginmonth, DbType.Int32));
            parameter.Add(db.NewParameter("Enddate", endmonth, DbType.Int32));
            DataTable dt = db.GetDataTable("sp_GetCreateCustomer", parameter);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRowReader reader = new DataRowReader(dt.Rows[i]);
                CustomerAmount detail = new CustomerAmount()
                {
                    Month = reader.GetInt32("amonth"),
                    CreatOrgCustomer = reader.GetInt32("orgcustomer"),
                    CreatPersonCustomer = reader.GetInt32("personcustomer")
                };
                result.Add(detail);
            }
            return result;
        }

        /// <summary>
        /// 获取活跃用户情况
        /// </summary>
        /// <param name="da.BeginMonth">开始月份</param>
        /// <param name="da.EndMonth">结束月份</param>
        /// <returns>月份、活跃机构用户数、活跃个人用户数、用户总数</returns>
        public static RecordResult<CustomerAmount> GetAliveCustomer(CustomerAmount da)
        {
            RecordResult<CustomerAmount> result = new RecordResult<CustomerAmount>();
            List<DbParameter> parameter = new List<DbParameter>();
            var db = DbFactory.Create();
            int beginmonth = TranslateHelper.ConvertDate(da.BeginMonth);
            int endmonth = TranslateHelper.ConvertDate(da.EndMonth);
            parameter.Add(db.NewParameter("Begindate", beginmonth, DbType.Int32));
            parameter.Add(db.NewParameter("Enddate", endmonth, DbType.Int32));
            var tnumcustomer = db.NewParameter("NumCustomer", 0, DbType.Int32);
            tnumcustomer.Direction = ParameterDirection.Output;
            parameter.Add(tnumcustomer);
            DataTable dt = db.GetDataTable("sp_GetAliveCustomer", parameter);

            result.List = new List<CustomerAmount>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRowReader reader = new DataRowReader(dt.Rows[i]);
                CustomerAmount detail = new CustomerAmount()
                {
                    Month = reader.GetInt32("amonth"),
                    AliveOrgCustomer = reader.GetInt32("orgcustomer"),
                    AlivePersonCustomer = reader.GetInt32("personcustomer")
                };
                result.List.Add(detail);
            }
            result.TotalRecords = DataConvert.ToInt32(tnumcustomer.Value);

            return result;

        }
    }
}
