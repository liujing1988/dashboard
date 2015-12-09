using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashBoard.Common
{
    /// <summary>
    /// 信用交易实体
    /// 刘静
    /// 2015-12-02
    /// </summary>
    public class CreditTrade
    {
        /// <summary>
        /// 股票名称
        /// </summary>
        public string StockName { get; set; }

        /// <summary>
        /// 交易量
        /// </summary>
        public string MatchQty { get; set; }

        /// <summary>
        /// 交易日期
        /// </summary>
        public string TradeDate { get; set; }
    }
}
