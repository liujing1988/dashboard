using Dashboard.Common;
using DashBoard.Common;
using DashBoard.Data;
using DashBoard.Logic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Logic
{
    public class DataServiceHelper
    {
        public static List<CustomerAmount> GetCustomer(CustomerAmount da)
        {
            List<CustomerAmount> result = new List<CustomerAmount>();
            int begindate = Int32.Parse(da.BeginMonth.Replace("-", ""));
            int enddate = Int32.Parse(da.EndMonth.Replace("-", ""));
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
        /// <returns></returns>
        public static List<TradeMonthAmount> GetMatchAmount(GetDateTime dateTime)
        {
            int bmonth = ConvertDate(dateTime.begindate);
            int emonth = ConvertDate(dateTime.enddate);
            List<TradeMonthAmount> result = new List<TradeMonthAmount>();
            using (var db = new ModelDataContainer())
            {
                var query = from a in db.strategytrade
                            where a.tradedate > 0 && (a.matchtype == "0" || a.matchtype == "1")
                            where a.tradedate / 100 >= bmonth && a.tradedate / 100 <= emonth
                            group a by a.tradedate / 100 into b
                            select new
                            {
                                Month = b.Max(p => p.tradedate / 100),
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
        /// 获取每日交易量
        /// </summary>
        /// <returns></returns>
        public static List<TradeMonthAmount> GetDateAmount(GetDateTime dateTime)
        {
            int bday = ConvertDate(dateTime.begindate);
            List<TradeMonthAmount> result = new List<TradeMonthAmount>();
            using (var db = new ModelDataContainer())
            {
                var query = from a in db.strategytrade
                            where a.tradedate > 0 && (a.matchtype == "0" || a.matchtype == "1")
                            where a.tradedate / 100 == bday
                            group a by a.tradedate into b
                            select new
                            {
                                Date = b.Max(p => p.tradedate),
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
        /// 获取每分钟交易量
        /// </summary>
        /// <returns></returns>
        public static List<RealTimeData> GetRealTimeData()
        {
            List<RealTimeData> result = new List<RealTimeData>();
            using (var db = new ModelDataContainer())
            {
                var query = (from a in db.strategytrade
                             where a.tradedate > 0 && (a.matchtype == "0" || a.matchtype == "1")
                             group a by (a.tradedate + (a.tradetime / 10000) / 10000.0) into b
                             select new
                             {
                                 Minute = b.Max(p => p.tradedate + (p.tradetime / 10000) / 10000.0),
                                 TradeAmount = b.Sum(p => p.matchamt)
                             }).OrderByDescending(p => p.Minute).Take(20);
                foreach (var item in query.OrderBy(p => p.Minute))
                {
                    result.Add(new RealTimeData()
                    {
                        Minute = ConvertMinute(item.Minute),
                        TradeAmount = item.TradeAmount,
                    });
                }
            }
            return result;
        }
        /// <summary>
        /// 获取功能模块使用量
        /// </summary>
        /// <returns></returns>
        public static List<StrategyTypes> GetStrategyType(GetDateTime dateTime)
        {
            int bdate = ConvertDate(dateTime.begindate);
            int edate = ConvertDate(dateTime.enddate);
            List<StrategyTypes> result = new List<StrategyTypes>();
            using (var db = new ModelDataContainer())
            {
                var query = (from a in db.strategyorder
                             where (a.strategytype == "0" || a.strategytype == "1"
                             || a.strategytype == "2" || a.strategytype == "3" || a.strategytype == "4")
                             where (a.orderdate >= bdate && a.orderdate <= edate)
                             group a by a.strategytype into h
                             select new
                             {
                                 strategytype = h.Max(p => p.strategytype),
                                 Nstrage = h.Count()
                             }
                    );
                foreach (var item in query.OrderBy(p => p.strategytype))
                {
                    if (item.strategytype != " ")
                    {
                        result.Add(new StrategyTypes()
                        {
                            StrategyType = ConvertStrategyType(item.strategytype),
                            NumStrategyType = item.Nstrage

                        });
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 获取用户日交易总量前五
        /// </summary>
        /// <returns></returns>
        public static List<TradeDayAmount> GetDayAmount()
        {
            int tdate = Int32.Parse(DateTime.Now.AddYears(-1).ToString("yyyyMMdd"));
            List<TradeDayAmount> result = new List<TradeDayAmount>();
            using (var db = new ModelDataContainer())
            {
                var query = (from a in db.strategytrade
                             where a.tradedate == tdate
                             group a by (a.custid) into b
                             select new
                             {
                                 custid = b.Max(p => p.custid),
                                 tradeamount = b.Sum(p => p.matchamt)
                             }).OrderByDescending(p => p.tradeamount).Take(5);
                foreach (var item in query)
                    result.Add(new TradeDayAmount()
                    {
                        CustId = item.custid,
                        TradeAmount = item.tradeamount,
                        TradeDate = DateTime.Now.AddYears(-1).ToString("yyyy-MM-dd")
                    });
            }
            return result;
        }

        /// <summary>
        /// 获取融资卖出交易标的前十
        /// </summary>
        /// <returns></returns>
        public static List<CreditTrade> GetCreditSalesAmount()
        {
            int tdate = Int32.Parse(DateTime.Now.AddYears(-1).ToString("yyyyMMdd"));
            List<CreditTrade> result = new List<CreditTrade>();
            using (var db = new ModelDataContainer())
            {
                var query = (from a in db.strategyorder
                             where a.orderdate == tdate
                             && a.matchqty > 0 && a.bsflag == "S"
                             group a by (a.stkcode) into b
                             select new
                             {
                                 stockcode = b.Max(p => p.stkcode),
                                 tradeamount = b.Sum(p => p.matchqty)
                             }).OrderByDescending(p => p.tradeamount).Take(10);
                StockDicManager.LoadDict();
                foreach (var item in query)
                    result.Add(new CreditTrade()
                    {
                        StockName = StockDicManager.GetStockName(item.stockcode.ToString()),
                        MatchQty = item.tradeamount.ToString(),
                        TradeDate = DateTime.Now.AddYears(-1).ToString("yyyy-MM-dd")
                    });
            }
            return result;
        }

        /// <summary>
        /// 获取融资买入交易标的前十
        /// </summary>
        /// <returns></returns>
        public static List<CreditTrade> GetCreditBuyAmount()
        {
            int tdate = Int32.Parse(DateTime.Now.AddYears(-1).ToString("yyyyMMdd"));
            List<CreditTrade> result = new List<CreditTrade>();
            using (var db = new ModelDataContainer())
            {
                var query = (from a in db.strategyorder
                             where a.orderdate == tdate
                             && a.matchqty > 0 && a.bsflag == "B"
                             group a by (a.stkcode) into b
                             select new
                             {
                                 stockcode = b.Max(p => p.stkcode),
                                 tradeamount = b.Sum(p => p.matchqty)
                             }).OrderByDescending(p => p.tradeamount).Take(10);
                StockDicManager.LoadDict();
                foreach (var item in query)
                    result.Add(new CreditTrade()
                    {
                        StockName = StockDicManager.GetStockName(item.stockcode.ToString()),
                        MatchQty = item.tradeamount.ToString(),
                        TradeDate = DateTime.Now.AddYears(-1).ToString("yyyy-MM-dd")
                    });
            }
            return result;
        }

        /// <summary>
        /// 获取客户交易流水
        /// </summary>
        /// <param name="da">搜索的起止时间</param>
        /// <returns></returns>
        public static List<TradeDetails> GetTradeDetails(TradeDetails da)
        {
            List<TradeDetails> result = new List<TradeDetails>();
            if (da == null)
            {
                using (var db = new ModelDataContainer())
                {
                    var query = (from a in db.strategytrade
                                 select new
                                 {
                                     custId = a.custid,
                                     tradeDate = a.tradedate,
                                     tradeTime = a.tradetime,
                                     stockCode = a.stkcode,
                                     matchQty = a.matchqty,
                                     matchPrice = a.matchprice,
                                     matchAmt = a.matchamt,
                                     bsflag = a.bsflag

                                 }
                            ).OrderByDescending(p => p.tradeDate);
                    foreach (var item in query)
                    {

                        result.Add(new TradeDetails()
                        {
                            CustId = item.custId.ToString(),
                            TradeDate = ConvertMinute(item.tradeDate),
                            TradeTime = item.tradeTime.ToString(),
                            StockCode = item.stockCode,
                            MatchQty = item.matchQty.ToString(),
                            MatchPrice = item.matchPrice.ToString(),
                            MatchAmt = item.matchAmt,
                            BsFlag = ConvertFlag(item.bsflag)
                        });

                    }
                }
            }
            else
            {
                int bdate = ConvertDate(da.begindate);
                int edate = ConvertDate(da.enddate);
                if (da.CustId != null)
                {
                    //int[] custid = ConvertArray(da.CustId.Split(','));
                    int custid = Int32.Parse(da.CustId);
                    using (var db = new ModelDataContainer())
                    {
                        var query = (from a in db.strategytrade
                                     where (a.tradedate >= bdate && a.tradedate <= edate)
                                     where a.custid == custid
                                     select new
                                     {
                                         custId = a.custid,
                                         tradeDate = a.tradedate,
                                         tradeTime = a.tradetime,
                                         stockCode = a.stkcode,
                                         matchQty = a.matchqty,
                                         matchPrice = a.matchprice,
                                         matchAmt = a.matchamt,
                                         bsflag = a.bsflag

                                     }
                                ).OrderByDescending(p => p.tradeDate);
                        foreach (var item in query)
                        {

                            result.Add(new TradeDetails()
                            {
                                CustId = item.custId.ToString(),
                                TradeDate = ConvertMinute(item.tradeDate),
                                TradeTime = item.tradeTime.ToString(),
                                StockCode = item.stockCode,
                                MatchQty = item.matchQty.ToString(),
                                MatchPrice = item.matchPrice.ToString(),
                                MatchAmt = item.matchAmt,
                                BsFlag = ConvertFlag(item.bsflag)
                            });

                        }
                    }
                }
                else
                {
                    using (var db = new ModelDataContainer())
                    {
                        var query = (from a in db.strategytrade
                                     where (a.tradedate >= bdate && a.tradedate <= edate)
                                     select new
                                     {
                                         custId = a.custid,
                                         tradeDate = a.tradedate,
                                         tradeTime = a.tradetime,
                                         stockCode = a.stkcode,
                                         matchQty = a.matchqty,
                                         matchPrice = a.matchprice,
                                         matchAmt = a.matchamt,
                                         bsflag = a.bsflag

                                     }
                                ).OrderByDescending(p => p.tradeDate);
                        foreach (var item in query)
                        {

                            result.Add(new TradeDetails()
                            {
                                CustId = item.custId.ToString(),
                                TradeDate = ConvertMinute(item.tradeDate),
                                TradeTime = item.tradeTime.ToString(),
                                StockCode = item.stockCode,
                                MatchQty = item.matchQty.ToString(),
                                MatchPrice = item.matchPrice.ToString(),
                                MatchAmt = item.matchAmt,
                                BsFlag = ConvertFlag(item.bsflag)
                            });

                        }
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 获取本月策略开仓金额
        /// </summary>
        /// <returns></returns>
        public static List<StrategyTypes> GetStrategyTradeAmt()
        {
            List<StrategyTypes> result = new List<StrategyTypes>();
            int tdate = Int32.Parse(DateTime.Now.AddYears(-1).ToString("yyyyMMdd"));
            using (var db = new ModelDataContainer())
            {
                var query = (from a in db.strategytrade
                             from b in db.strategyinfo
                             where a.strategyno == b.strategyno && a.tradedate / 100 == tdate / 100
                             group a by new { b.strategyname } into c
                             select new
                             {
                                 strategyname = c.Key.strategyname,
                                 strategytradeamt = c.Sum(p => p.matchamt)
                             }).OrderByDescending(p => p.strategytradeamt).Take(3);

                foreach (var item in query)
                {
                    result.Add(new StrategyTypes()
                    {
                        StrategyType = item.strategyname,
                        StrategyTradeAmt = item.strategytradeamt
                    });
                }
            }
            return result;
        }

        /// <summary>
        /// 获取本月策略开仓次数
        /// </summary>
        /// <returns></returns>
        public static List<StrategyTypes> GetStrategyTradeAct()
        {
            List<StrategyTypes> result = new List<StrategyTypes>();
            int tdate = Int32.Parse(DateTime.Now.AddYears(-1).ToString("yyyyMMdd"));
            using (var db = new ModelDataContainer())
            {
                var query = (from a in db.strategytrade
                             from b in db.strategyinfo
                             where a.strategyno == b.strategyno && a.tradedate / 100 == tdate / 100
                             group a by new { b.strategyname } into c
                             select new
                             {
                                 strategyname = c.Key.strategyname,
                                 strategytradenum = c.Count()
                             }).OrderByDescending(p => p.strategytradenum).Take(5);

                foreach (var item in query)
                {
                    result.Add(new StrategyTypes()
                    {
                        StrategyType = item.strategyname,
                        NumStrategyType = item.strategytradenum
                    });
                }
            }
            return result;
        }

        /// <summary>
        /// 获取当前在线用户数和信用用户数
        /// </summary>
        /// <returns></returns>
        public static CustomerOnline GetCustomerOnline()
        {
            CustomerOnline result = new CustomerOnline();
            int tdate = Int32.Parse(DateTime.Now.AddYears(-1).ToString("yyyyMMdd"));
            int ttime = Int32.Parse(DateTime.Now.ToString("HHmm"));
            using(var db = new ModelDataContainer())
            {
                var query = from a in db.strategyorder
                            from b in db.custacctinfo
                            where a.custid == b.custid && a.orderdate == tdate 
                            && (b.tradetype == "0" || b.tradetype == "1")
                            group a by new{b.tradetype,a.custid} into c 
                            group c by c.Key.tradetype into d
                            select new
                            {
                                numcust = d.Count(),
                                creditcust = d.Count(p=>p.Key.tradetype == "1")
                            };
                foreach (var item in query)
                {
                    result.NumCustomerOnline = item.numcust;
                    result.CreditCustomer = item.creditcust;
                    result.ServerDate = DateTime.Now.AddYears(-1).ToString("yyyy-MM-dd");
                }
            }
            return result;
        }

        /// <summary>
        /// 获取当日委托、成交、撤单情况
        /// </summary>
        /// <returns></returns>
        public static TradeDayVolume GetTradeDayVolume()
        {
            TradeDayVolume result = new TradeDayVolume();
            int tdate = Int32.Parse(DateTime.Now.AddYears(-1).ToString("yyyyMMdd"));
            int ttime = Int32.Parse(DateTime.Now.ToString("HHmm"));
            using (var db = new ModelDataContainer())
            {
                var ordercount = from a in db.strategyorder
                                 where a.orderdate == tdate && a.cancelflag == "F"
                                 && a.orderstatus != "9"
                                 group a by a.orderdate into b
                                 select new
                                 {
                                     ordercount = b.Count()
                                 };
                foreach (var item in ordercount)
                {
                    result.NumOrder = item.ordercount;
                }

                var volumecount = from a in db.strategyorder
                                 where a.orderdate == tdate && a.cancelflag == "F"
                                 && a.orderstatus != "9" && a.matchqty > 0
                                 group a by a.orderdate into b
                                  select new
                                  {
                                      volumecount = b.Count()
                                  };
                foreach (var item in volumecount)
                {
                    result.NumVolum = item.volumecount;
                }
                            
                var cancelcount = from a in db.strategyorder
                               where a.orderdate == tdate && a.cancelflag == "T"
                               && a.orderstatus != "9" && a.matchqty > 0
                               group a by a.orderdate into b
                                  select new
                                  {
                                      cancelcount = b.Count()
                                  };
                foreach (var item in cancelcount)
                {
                    result.NumRevoke = item.cancelcount;
                }

                var volumeamt = from a in db.strategyorder
                                where a.orderdate == tdate && a.cancelflag == "F"
                                && a.orderstatus != "9" && a.matchqty > 0
                                group a by a.orderdate into b
                                select new { 
                                volumeamt = b.Sum(p=>p.matchamt)
                                };
                foreach (var item in volumeamt)
                {
                    result.VolumAmt = item.volumeamt.ToString();
                }

                var rrpamt = from a in db.strategyorder
                                where a.orderdate == tdate && a.cancelflag == "F"
                                && a.orderstatus != "9" && a.matchqty > 0
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
                var buyamt = from a in db.strategyorder
                             where a.orderdate == tdate && a.cancelflag == "F"
                             && a.orderstatus != "9" && a.matchqty > 0
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
                var salesamt = from a in db.strategyorder
                             where a.orderdate == tdate && a.cancelflag == "F"
                             && a.orderstatus != "9" && a.matchqty > 0
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
        /// 交易类型转换
        /// </summary>
        /// <param name="strategytype"></param>
        /// <returns></returns>
        public static string ConvertStrategyType(string strategytype)
        {
            string result = null;

            switch (strategytype)
            {
                case "0":
                    result = "普通";
                    break;
                case "1":
                    result = "TWAP";
                    break;
                case "2":
                    result = "时序";
                    break;
                case "3":
                    result = "盘口";
                    break;
                case "4":
                    result = "ETF";
                    break;
            }
            return result;
        }

        /// <summary>
        /// 实时数据json时间格式转换
        /// </summary>
        /// <param name="Minute">传入时间，格式为yyyyMMdd.HHmiss或者yyyyMMdd</param>
        /// <returns></returns>
        public static string ConvertMinute(double Minute)
        {
            string result;
            string[] date = null;
            if (Minute.ToString().Contains('.'))
            {
                date = Minute.ToString().Split('.');

                while (date[1].Length < 6)
                {
                    date[1] = date[1] + "0";
                }
                string datetime = date[0].Substring(0, 4) + "-" + date[0].Substring(4, 2) +
                    "-" + date[0].Substring(6, 2) + " " + date[1].Substring(0, 2) + ":" +
                    date[1].Substring(2, 2) + ":" + date[1].Substring(4, 2);
                result = datetime;
            }
            else
            {
                string sdate = Minute.ToString().Substring(0, 4) + "-" + Minute.ToString().Substring(4, 2) +
                         "-" + Minute.ToString().Substring(6, 2);
                result = sdate;
            }

            return result;
        }

        /// <summary>
        /// 前台页面传递过来的时间转换
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static int ConvertDate(string datetime)
        {
            int date = -1;
            if (datetime != null)
            {
                string[] DT = datetime.Split('-');
                string dt = null;
                for (int i = 0; i < DT.Length; i++)
                {
                    if (DT[i].Length < 2)
                    {
                        DT[i] = "0" + DT[i];
                    }
                    dt += DT[i];
                }

                date = Int32.Parse(dt);
            }
            return date;
        }
        /// <summary>
        /// 交易方向转换
        /// </summary>
        /// <param name="BSFlag"></param>
        /// <returns></returns>
        public static string ConvertFlag(string BSFlag)
        {
            string result = null;
            if (BSFlag == "S")
            {
                result = "卖出";
            }
            else if (BSFlag == "B")
            {
                result = "买入";
            }

            return result;
        }

        /// <summary>
        /// 将字符串型数组转换为整形数组
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        //public static int[] ConvertArray(params string[] arr)
        //{
        //    int[] result = null;
        //    for (int i = 0;i < arr.Length;i++)
        //    {
        //        result[i] = Convert.ToInt32(arr[i]);
        //    }
        //    return result;
        //}
    }
}
