using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Common
{
    /// <summary>
    /// 交易量参数传递
    /// 作者：刘静
    /// 修改日期：2015-11-30
    /// </summary>
    public class RealTimeData
    {
        /// <summary>
        /// 时间，精确到分钟
        /// </summary>
        public string Minute { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        public string Day { get; set; }

        /// <summary>
        /// 交易量
        /// </summary>
        public decimal TradeAmount { get; set; }

        /// <summary>
        /// 查询开始日期
        /// </summary>
        public string BeginDate { get; set; }

        /// <summary>
        /// 查询结束日期
        /// </summary>
        public string EndDate { get; set; }
    }
}
