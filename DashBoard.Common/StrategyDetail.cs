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
        public string BeginDate { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public string EndDate { get; set; }

        /// <summary>
        /// 策略名
        /// </summary>
        public string StrategyName { get; set; }

        /// <summary>
        /// 策略类型名
        /// </summary>
        public string StrategyKindName { get; set; }

        /// <summary>
        /// 策略描述
        /// </summary>
        public string StratInfo { get; set; }

        /// <summary>
        /// 系列号
        /// </summary>
        public string SeriesNo { get; set; }

        /// <summary>
        ///策略组
        /// </summary>
        public string StrategyGroup { get; set; }

        /// <summary>
        ///开仓时间
        /// </summary>
        public string CreateDate { get; set; }

        /// <summary>
        /// 搜索内容
        /// </summary>
        public string SearchColumns { get; set; }

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
        public string SortDirection { get; set; }

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

        /// <summary>
        /// 自动上传交易量
        /// </summary>
        public decimal AutoAmt { get; set; }

        /// <summary>
        /// 手动上载交易量
        /// </summary>
        public decimal ManualAmt { get; set; }

        /// <summary>
        /// 客户ID
        /// </summary>
        public string CustId { get; set; }

        /// <summary>
        /// 开仓时间
        /// </summary>
        public string CreateTime { get; set; }

        /// <summary>
        /// 股票代码
        /// </summary>
        public string StockCode { get; set; }

        /// <summary>
        /// 股票名称
        /// </summary>
        public string StockName { get; set; }

        /// <summary>
        /// 交易方向
        /// </summary>
        public string BSFlag { get; set; }

        /// <summary>
        /// 委托量
        /// </summary>
        public string OrderQty { get; set; }
    }
}
