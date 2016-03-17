using Dashboard.Common;
using Dashboard.Logic;
using DashBoard.Common;
using DashBoard.Web.Areas.HealthIndex.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DashBoard.Web.Areas.TradeData.Controllers
{
    /// <summary>
    /// 交易API
    /// 作者：刘静
    /// 修改时间：20151127
    /// </summary>
    public class GetTradeDateController : ApiController
    {
        /// <summary>
        /// 发布月交易金额API
        /// </summary>
        /// <param name="dateTime">起止月份</param>
        /// <returns></returns>
        [HttpPost]
        public IEnumerable<TradeMonthAmount> GetMatchAmount(StrategyDetail dateTime)
        {
            return DataServiceHelper.GetMatchAmount(dateTime);
        }

        /// <summary>
        /// 发布月开仓次数API
        /// </summary>
        /// <param name="dateTime">起止月份</param>
        /// <returns></returns>
        [HttpPost]
        public IEnumerable<OrderSend> GetOrderSendNum(StrategyDetail dateTime)
        {
            return DataServiceHelper.GetOrderSendNum(dateTime);
        }

        /// <summary>
        /// 发布策略明细中符合条件的交易量前五客户情况API
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        [HttpPost]
        public IEnumerable<TopMatchQty> GetTopMatchAmt(StrategyDetail dateTime)
        {
            return DataServiceHelper.GetTopMatchAmt(dateTime);
        }

        /// <summary>
        /// 发布单月中每日交易量API
        /// </summary>
        /// <param name="dateTime">月份</param>
        /// <returns></returns>
        [HttpPost]
        public IEnumerable<TradeMonthAmount> GetDateAmount(StrategyDetail dateTime)
        {
            return DataServiceHelper.GetDateAmount(dateTime);
        }

        /// <summary>
        /// 获取当日最新成交量
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public List<RealTimeData> GetRealTimeData()
        {
            return DataServiceHelper.GetRealTimeData();
            //RealTimeData da = new RealTimeData();
            //da.beginDate = DateTime.Now.AddDays(-5).ToString("yyyy-MM-dd");
            //da.endDate = DateTime.Now.ToString("yyyy-MM-dd");
            //return DataServiceHelper.GetRealData(da);
        }

        /// <summary>
        /// 综合查询中按时间查询成交量
        /// </summary>
        /// <param name="da">起止日期</param>
        /// <returns></returns>
        [HttpPost]
        public List<RealTimeData> GetRealData(RealTimeData da)
        {
            return DataServiceHelper.GetRealData(da);
        }

        /// <summary>
        /// 获取当日成交量前五
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public List<TradeDayAmount> GetDayAmount()
        {
            return DataServiceHelper.GetDayAmount();
        }

        /// <summary>
        /// 获取融券卖出交易标的前十
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public List<CreditTrade> GetCreditSalesAmount(RealTimeData da)
        {
            List<CreditTrade> result = new List<CreditTrade>();
            result = DataServiceHelper.GetCreditSalesAmount(da);
            if (result.Count != 0)
            {
                result[0].RefreshRate = Int32.Parse(IndexManagers.ReadConfig().IndexRefreshRate);
            }
            return result;
        }

        /// <summary>
        /// 获取融资买入交易标的前十
        /// </summary>
        /// <returns></returns>
        //[HttpPost]
        //public List<CreditTrade> GetCreditBuyAmount(RealTimeData da)
        //{
        //    List<CreditTrade> result = new List<CreditTrade>();
        //    result = DataServiceHelper.GetCreditBuyAmount(da);
        //    if (result.Count != 0)
        //    {
        //        result[0].RefreshRate = Int32.Parse(IndexManagers.ReadConfig().IndexRefreshRate);
        //    }
        //    return result;
        //}

        /// <summary>
        /// 获取当前月策略开仓金额前三
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public List<Modules> GetStrategyTradeAmt()
        {
            return DataServiceHelper.GetStrategyTradeAmt();
        }

        /// <summary>
        /// 获取当前月策略开仓次数前五
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public List<Modules> GetStrategyTradeAct()
        {
            return DataServiceHelper.GetStrategyTradeAct();
        }

        /// <summary>
        /// 获取当日委托笔数、成交笔数、撤单笔数
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public TradeDayVolume GetTradeDayVolume()
        {
            TradeDayVolume result = new TradeDayVolume();
            result = DataServiceHelper.GetTradeDayVolume();

            HealthManagers config = new HealthManagers();
            config = IndexManagers.ReadConfig();
            result.ThDayNumOrder = Int32.Parse(config.MaxDayOrder);
            result.ThMiNumOrder = Int32.Parse(config.MaxMinuteOrder);
            result.ThSeNumOrder = Int32.Parse(config.MaxSecondOrder);
            return result;
        }

        /// <summary>
        /// 用户自建策略交易量情况（包括自动上传和手动上载）
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public List<StrategyDetail> GetCustomerCreatedStrategy(StrategyDetail da)
        {
            return DataServiceHelper.GetCustomerCreatedStrategy(da);
        }

        /// <summary>
        /// 发布用户自建策略交易量前五客户情况API
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        [HttpPost]
        public IEnumerable<TopMatchQty> GetCustomerCreateStrategyTopMatchQty(StrategyDetail dateTime)
        {
            return DataServiceHelper.GetCustomerCreateStrategyTopMatchQty(dateTime);
        }

        [HttpPost]
        public List<StrategyDetail> GetSubStrategyTradeDetail(StrategyDetail da)
        {
            return DataServiceHelper.GetSubStrategyTradeDetail(da);
        }
    }
}
