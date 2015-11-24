using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Common
{
    public class TradeDetails
    {
        //客户代码
        public  string CustId { get; set; }
        //交易日期
        public  string TradeDate { get; set; }
        //交易时间
        public  string TradeTime { get; set; }
        //股票代码
        public  string StockCode { get; set; }
        //成交量
        public string MatchQty { get; set; }
        //成交价格
        public string MatchPrice { get; set; }
        //成交金额
        public  decimal MatchAmt { get; set; }
        //交易方向
        public  string BsFlag { get; set; }
        //查询开始时间
        public string begindate { get; set; }
        //查询结束时间
        public string enddate { get; set; }
        public string searchColumns { get; set; }
        public int DisplayStart { get; set; }
        public int DisplayLength { get; set; }
        public string sortDirection { get; set; }
        public int PageCount { get; set; }
        public int CurrentPage { get; set; }
        public int TotalRecords { get; set; }
        public int TotalDisplayRecords { get; set; }
        public string OrderField { get; set; }
    }
}
