using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashBoard.Common
{
    public class TradeDayVolume
    {
        //日委托笔数
        public int NumOrder { get; set; }

        //日交易量阈值
        public int ThDayNumOrder { get; set; }

        //分钟交易量阈值
        public int ThMiNumOrder { get; set; }

        //秒交易量阈值
        public int ThSeNumOrder { get; set; }

        //分钟委托笔数
        public int MiNumOrder { get; set; }

        //秒委托笔数
        public int SeNumOrder { get; set; }

        //成交笔数
        public int NumVolum { get; set; }

        //撤单笔数
        public int NumRevoke { get; set; }

        //成交金额
        public string VolumAmt { get; set; }

        //逆回购金额
        public string RRPAmt { get; set; }

        //买入金额
        public string BuyAmt { get; set; }

        //卖出金额
        public string SalesAmt { get; set; }

        //撤单比值
        public decimal PerRevoke { get; set; }

        //交易日期
        public decimal OrderDate { get; set; }

        //客户ID
        public int CustId { get; set; }
    }
}
