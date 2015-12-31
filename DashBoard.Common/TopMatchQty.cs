using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashBoard.Common
{
    public class TopMatchQty
    {
        /// <summary>
        /// 客户ID
        /// </summary>
        public string CustId { get; set; }

        /// <summary>
        /// 策略名
        /// </summary>
        public string StrategyName { get; set; }

        /// <summary>
        /// 交易总量
        /// </summary>
        public Nullable<int> MatchQty { get; set; }
    }
}
