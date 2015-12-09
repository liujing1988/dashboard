using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Common
{
    /// <summary>
    /// 交易明细类
    /// 刘静
    /// 2015-12-03
    /// </summary>
    public class TradeDetails
    {
        /// <summary>
        /// 客户代码
        /// </summary>
        public  string CustId { get; set; }

        /// <summary>
        /// 交易日期
        /// </summary>
        public  string TradeDate { get; set; }

        /// <summary>
        /// 交易/委托比
        /// </summary>
        public string PercentRevoke { get; set; }

        /// <summary>
        /// 撤单笔数
        /// </summary>
        public string NumRevoke { get; set; }

        /// <summary>
        /// 委托笔数
        /// </summary>
        public string NumOrder { get; set; }

        /// <summary>
        /// 交易时间
        /// </summary>
        public  string TradeTime { get; set; }
        
        /// <summary>
        /// 股票代码
        /// </summary>
        public  string StockCode { get; set; }

        /// <summary>
        /// 成交量
        /// </summary>
        public string MatchQty { get; set; }

        /// <summary>
        /// 委托日期
        /// </summary>
        public string OrderDate { get; set; }

        /// <summary>
        /// 委托/撤单时间
        /// </summary>
        public string OperTime { get; set; }
        
        /// <summary>
        /// 委托量
        /// </summary>
        public string OrderQty { get; set; }

        /// <summary>
        /// 撤单标识
        /// </summary>
        public string CancelFlag { get; set; }

        /// <summary>
        /// 成交价格
        /// </summary>
        public string MatchPrice { get; set; }

        /// <summary>
        /// 成交金额
        /// </summary>
        public  decimal MatchAmt { get; set; }

        /// <summary>
        /// 交易方向
        /// </summary>
        public  string BsFlag { get; set; }

        /// <summary>
        /// 查询开始时间
        /// </summary>
        public string begindate { get; set; }

        /// <summary>
        /// 查询结束时间
        /// </summary>
        public string enddate { get; set; }

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
        /// 获取撤单/委托比大于60%的客户id和比值
        /// </summary>
        public string LimitMinOrder { get; set; }

        //分钟委托笔数
        public string MiNumOrder { get; set; }

        //秒委托笔数
        public string SeNumOrder { get; set; }
    }
}
