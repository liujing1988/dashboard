using Dashboard.Common;
using DashBoard.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Logic
{
    public class DemoService
    {
        public static List<CustomerAmount> GetCustomer(CustomerAmount da)
        {
            List<CustomerAmount> result = new List<CustomerAmount>();
            int begindate = Int32.Parse(da.BeginMonth.Replace("-", ""));
            int enddate = Int32.Parse(da.EndMonth.Replace("-", ""));
            using (var db = new ModelDataContainer())
            {
                var list = db.sp_GetCustomer(begindate,enddate);
                foreach(var item in list)
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
                            where a.tradedate/100 >= bmonth && a.tradedate/100 <= emonth
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
                            where a.tradedate/100 == bday 
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
                            group a by (a.tradedate + (a.tradetime / 10000)/10000.0) into b
                            select new
                            {
                                Minute = b.Max(p => p.tradedate + (p.tradetime / 10000)/10000.0),
                                TradeAmount = b.Sum(p => p.matchamt)
                            }).OrderByDescending(p=>p.Minute).Take(20);
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
                            MatchQty = item.matchQty,
                            MatchPrice = item.matchPrice,
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
                                MatchQty = item.matchQty,
                                MatchPrice = item.matchPrice,
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
                                MatchQty = item.matchQty,
                                MatchPrice = item.matchPrice,
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
            if(BSFlag == "S")
            {
                result = "卖出";
            }
            else if(BSFlag == "B")
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
