using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashBoard.Common
{
    /// <summary>
    /// 平台健康指数阈值设置实体
    /// 刘静
    /// 2015-12-02
    /// </summary>
    public class HealthManagers
    {
        /// <summary>
        /// 日最大委托数
        /// </summary>
        public string MaxDayOrder { get; set; }

        /// <summary>
        /// 每分钟最大委托数
        /// </summary>
        public string MaxMinuteOrder { get; set; }

        /// <summary>
        /// 每秒最大委托数
        /// </summary>
        public string MaxSecondOrder { get; set; }

        /// <summary>
        /// 每日撤单委托比大于60%用户的最小委托笔数
        /// </summary>
        public string LimitMinOrder { get; set; }
    }
}
