using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashBoard.Common
{
    /// <summary>
    /// 日交易量
    /// </summary>
    public class TradeDayAmount
    {
        /// <summary>
        /// 交易日期
        /// </summary>
        public string TradeDate { get; set; }

        /// <summary>
        /// 交易量
        /// </summary>
        public decimal TradeAmount { get; set; }

        /// <summary>
        /// 信用交易
        /// </summary>
        public decimal CreditTrade { get; set; }

        /// <summary>
        /// 客户ID
        /// </summary>
        public int CustId { get; set; }
    }
}
