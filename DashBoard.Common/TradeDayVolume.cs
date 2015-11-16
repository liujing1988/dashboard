﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashBoard.Common
{
    public class TradeDayVolume
    {
        //委托笔数
        public int NumOrder { get; set; }
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
    }
}