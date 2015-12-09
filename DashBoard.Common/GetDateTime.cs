using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Common
{
    /// <summary>
    /// 时间获取类
    /// 刘静
    /// 2015-12-02
    /// </summary>
    public class GetDateTime
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        public string begindate { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public string enddate { get; set; }

        /// <summary>
        /// 获取撤单/委托比大于60%的客户id和比值
        /// </summary>
        public string LimitMinOrder { get; set; }

    }
}
