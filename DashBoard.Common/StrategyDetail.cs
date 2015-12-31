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
        /// 策略类型名
        /// </summary>
        public string strategyKindName { get; set; }

        /// <summary>
        /// 策略描述
        /// </summary>
        public string stratInfo { get; set; }

        /// <summary>
        /// 系列号
        /// </summary>
        public string seriesNo { get; set; }

        /// <summary>
        ///策略组
        /// </summary>
        public string strategyGroup { get; set; }

        /// <summary>
        ///开仓时间
        /// </summary>
        public string createDate { get; set; }

        /// <summary>
        /// 搜索内容
        /// </summary>
        public string searchColumns { get; set; }

        /// <summary>
        /// 当前页第一条记录是总记录数的第几条记录
        /// </summary>
        public int DisplayStart { get; set; }

        /// <summary>
        /// 每页显示条数
        /// </summary>
        public int DisplayLength { get; set; }

        /// <summary>
        /// 排序方式（正序、倒序）
        /// </summary>
        public string sortDirection { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public int PageCount { get; set; }

        /// <summary>
        /// 当前页面
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// 总记录数
        /// </summary>
        public int TotalRecords { get; set; }

        /// <summary>
        /// 显示的总记录数
        /// </summary>
        public int TotalDisplayRecords { get; set; }

        /// <summary>
        /// 排序字段
        /// </summary>
        public string OrderField { get; set; }

        /// <summary>
        /// 模糊搜索内容
        /// </summary>
        public string sSearch { get; set; }

        /// <summary>
        /// 月份
        /// </summary>
        public int Month { get; set; }

        /// <summary>
        /// 自动上传交易量
        /// </summary>
        public int AutoQty { get; set; }

        /// <summary>
        /// 手动上载交易量
        /// </summary>
        public int ManualQty { get; set; }

        public string CustId { get; set; }

        public string CreateTime { get; set; }

        public string StockCode { get; set; }

        public string StockName { get; set; }

        public string BSFlag { get; set; }

        public string OrderQty { get; set; }
    }
}
