using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashBoard.Common
{
    public class TradeDayAmount
    {
        public string TradeDate { get; set; }
        public decimal TradeAmount { get; set; }
        public int CustId { get; set; }
    }
}
