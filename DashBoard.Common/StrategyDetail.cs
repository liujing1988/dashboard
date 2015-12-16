using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashBoard.Common
{
    /// <summary>
    /// 策略交易
    /// 刘静
    /// 2015-12-15
    /// </summary>
    public class StrategyDetail
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        public string beginDate { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public string endDate { get; set; }

        /// <summary>
        /// 策略名
        /// </summary>
        public string strategyName { get; set; }

        /// <summary>
        /// 策略号
        /// </summary>
        public string stratInfo { get; set; }

        /// <summary>
        /// 系列号
        /// </summary>
        public string seriesNo { get; set; }
    }
}
