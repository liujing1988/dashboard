using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashBoard.Common
{
    /// <summary>
    /// 当日交易情况
    /// </summary>
    public class TradeDayVolume
    {
        /// <summary>
        /// 日委托笔数
        /// </summary>
        public int NumOrder { get; set; }

        /// <summary>
        /// 日交易量阈值
        /// </summary>
        public int ThDayNumOrder { get; set; }

        /// <summary>
        /// 分钟交易量阈值
        /// </summary>
        public int ThMiNumOrder { get; set; }

        /// <summary>
        /// 秒交易量阈值
        /// </summary>
        public int ThSeNumOrder { get; set; }

        /// <summary>
        /// 分钟委托笔数
        /// </summary>
        public int MiNumOrder { get; set; }

        /// <summary>
        /// 秒委托笔数
        /// </summary>
        public int SeNumOrder { get; set; }

        /// <summary>
        /// 成交笔数
        /// </summary>
        public int NumVolum { get; set; }

        /// <summary>
        /// 撤单笔数
        /// </summary>
        public int NumRevoke { get; set; }

        /// <summary>
        /// 成交金额
        /// </summary>
        public string VolumAmt { get; set; }

        /// <summary>
        /// 逆回购金额
        /// </summary>
        public string RRPAmt { get; set; }

        /// <summary>
        /// 买入金额
        /// </summary>
        public string BuyAmt { get; set; }

        /// <summary>
        /// 卖出金额
        /// </summary>
        public string SalesAmt { get; set; }

        /// <summary>
        /// 撤单比值
        /// </summary>
        public decimal PerRevoke { get; set; }

        /// <summary>
        /// 交易日期
        /// </summary>
        public decimal OrderDate { get; set; }

        /// <summary>
        /// 客户ID
        /// </summary>
        public int CustId { get; set; }
    }
}
